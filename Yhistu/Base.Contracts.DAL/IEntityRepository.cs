using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

//TODO: Check for user ownerhip on everything except create
public interface IEntityRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    TEntity Remove(TKey id);

    TEntity? FirstOrDefault(TKey id, bool noTracking = true);
    IEnumerable<TEntity> GetAll(bool noTracking = true);
    bool Exists(TKey id);

    TKey GetIdFromDb(TEntity entity);

    TEntity GetUpdatedEntityAfterSaveChanges(TEntity entity);

    
    // Async methods
    
    Task<TEntity> RemoveAsync(TKey id);

    Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
    Task<bool> ExistsAsync(TKey id);
}