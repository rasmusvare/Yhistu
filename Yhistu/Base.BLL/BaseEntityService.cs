using System.Reflection;
using Base.Contracts.Base;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Base.Extensions;

namespace Base.BLL;

public class BaseEntityService<TBllEntity, TDalEntity, TRepository> :
    BaseEntityService<TBllEntity, TDalEntity, TRepository, Guid>,
    IEntityService<TBllEntity>
    where TDalEntity : class, IDomainEntityId
    where TBllEntity : class, IDomainEntityId
    where TRepository : IEntityRepository<TDalEntity>
{
    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository, mapper)
    {
    }
}

public class BaseEntityService<TBllEntity, TDalEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TRepository : IEntityRepository<TDalEntity, TKey>
    where TKey : IEquatable<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
{
    protected TRepository Repository;
    protected IMapper<TBllEntity, TDalEntity> Mapper;
    protected Dictionary<TBllEntity, TDalEntity> _entityCache = new();

    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public virtual TBllEntity Add(TBllEntity entity)
    {
        var dalEntity = Mapper.Map(entity)!;
        _entityCache.Add(entity, dalEntity);

        return Mapper.Map(Repository.Add(dalEntity))!;
    }

    public TBllEntity Update(TBllEntity entity)
    {
            TDalEntity dalEntity;
            if (!_entityCache.ContainsKey(entity))
            {
                dalEntity = Mapper.Map(entity)!;
                _entityCache.Add(entity, dalEntity);
            }
            else
            {
                dalEntity = _entityCache[entity];

                foreach (PropertyInfo each in dalEntity.GetType().GetFilteredProperties())
                {
                    var entityProperty = entity.GetType().GetProperty(each.Name);

                    if (entityProperty != null )
                    {
                        each.SetValue(dalEntity, entityProperty?.GetValue(entity));
                    }
                }
            }

            return Mapper.Map(Repository.Update(dalEntity))!;
    }

    public TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(Repository.Remove(Mapper.Map(entity)!))!;
    }

    public TBllEntity Remove(TKey id)
    {
        return Mapper.Map(Repository.Remove(id))!;
    }

    public TBllEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(Repository.FirstOrDefault(id, noTracking));
    }

    public IEnumerable<TBllEntity> GetAll(bool noTracking = true)
    {
        return Repository.GetAll(noTracking).Select(x => Mapper.Map(x)!);
    }

    public bool Exists(TKey id)
    {
        return Repository.Exists(id);
    }

    public TKey GetIdFromDb(TBllEntity entity)
    {
        var dalEntity = _entityCache[entity];

        return Repository.GetIdFromDb(dalEntity);
    }

    public TBllEntity GetUpdatedEntityAfterSaveChanges(TBllEntity entity)
    {
        var dalEntity = _entityCache[entity];
        var updatedDalEntity = Repository.GetUpdatedEntityAfterSaveChanges(dalEntity);
        var bllEntity = Mapper.Map(updatedDalEntity)!;

        return bllEntity;
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id, noTracking));
    }

    public async Task<IEnumerable<TBllEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAsync(noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Repository.ExistsAsync(id);
    }

    public async Task<TBllEntity> RemoveAsync(TKey id)
    {
        return Mapper.Map(await Repository.RemoveAsync(id))!;
    }
}