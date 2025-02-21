using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using llibrary.Guards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Source.Db.Repo;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TEntity?>> GetWhereAsync(
        Expression<Func<TEntity, bool>> predicate
    )
    {
        return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
        _transaction = null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _transaction?.Dispose();
        _transaction = null;
        _context.Dispose();
    }

    public IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        IRepository<TEntity> repo = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repo);
        return repo;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            Guard.AgainstNull(_transaction);

            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            await RollbackAsync();
            ExceptionDispatchInfo.Throw(exception);
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        Guard.AgainstNull(_transaction);

        await _transaction.RollbackAsync();
        _transaction?.Dispose();
        _transaction = null;
    }
}
