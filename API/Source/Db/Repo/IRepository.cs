using System.Linq.Expressions;

namespace API.Source.Db.Repo;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task<IEnumerable<TEntity?>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task SaveAsync();
}

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
