using System.Linq.Expressions;
using MongoDB.Driver;

namespace database.interfaces;

public interface IDatabaseAdapter<TEntity>
{
    Task CreateAsync(TEntity arg);
    Task CreateManyAsync(IEnumerable<TEntity> args);
    Task<TEntity> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<int> UpdateAsync(object id, TEntity arg);
    Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> filter,
        Dictionary<string, object> updateDefinition,
        bool isUpsert = true
    );
    Task<int> DeleteAsync(object id);
}
