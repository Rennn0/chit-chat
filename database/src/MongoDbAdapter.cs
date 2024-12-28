using System.Linq.Expressions;
using database.interfaces;
using MongoDB.Driver;

namespace database;

public class MongoDbAdapter<TEntity> : IDatabaseAdapter<TEntity>
{
    protected readonly IMongoCollection<TEntity> Collection;

    public MongoDbAdapter(IMongoDatabase mongo)
    {
        Collection = mongo.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public virtual async Task CreateAsync(TEntity arg)
    {
        await Collection.InsertOneAsync(arg);
    }

    public virtual async Task CreateManyAsync(IEnumerable<TEntity> args)
    {
        await Collection.InsertManyAsync(args);
    }

    public virtual async Task<TEntity> GetByIdAsync(object id)
    {
        FilterDefinition<TEntity>? filter = Builders<TEntity>.Filter.Eq("_id", id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetByAsync(
        Expression<Func<TEntity, bool>> filter
    )
    {
        return await Collection.Find(filter).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<int> UpdateAsync(object id, TEntity arg)
    {
        FilterDefinition<TEntity>? filter = Builders<TEntity>.Filter.Eq("_id", id);
        ReplaceOneResult? result = await Collection.ReplaceOneAsync(filter, arg);
        return result.IsAcknowledged ? (int)result.ModifiedCount : 0;
    }

    public virtual async Task<int> UpdateAsync(
        Expression<Func<TEntity, bool>> filter,
        Dictionary<string, object> updateDefinitions,
        bool isUpsert = true
    )
    {
        List<UpdateDefinition<TEntity>> updates = [];
        updates.AddRange(
            updateDefinitions.Select(update =>
                Builders<TEntity>.Update.AddToSet(update.Key, update.Value)
            )
        );

        UpdateOptions updaetOptions = new UpdateOptions() { IsUpsert = isUpsert };
        UpdateResult? result = await Collection.UpdateManyAsync(
            filter,
            Builders<TEntity>.Update.Combine(updates),
            updaetOptions
        );
        return result.IsAcknowledged ? (int)result.ModifiedCount : 0;
    }

    public virtual async Task<int> DeleteAsync(object id)
    {
        FilterDefinition<TEntity>? filter = Builders<TEntity>.Filter.Eq("_id", id);
        DeleteResult? result = await Collection.DeleteOneAsync(filter);
        return result.IsAcknowledged ? (int)result.DeletedCount : 0;
    }
}
