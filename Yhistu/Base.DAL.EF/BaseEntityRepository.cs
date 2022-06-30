using System.Reflection;
using System.Reflection.Metadata;
using Base.Contracts.Base;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Base.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Base.DAL.EF;

//TODO: Do not fetch unnecessary data from DB on every request - look into AutoMapper functionality

public class
    BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> : BaseEntityRepository<TDalEntity, TDomainEntity, Guid,
        TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext,
        mapper)
    {
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IMapper<TDalEntity, TDomainEntity> Mapper;

    private readonly Dictionary<TDalEntity, TDomainEntity> _entityCache = new();

    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = RepoDbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;
        _entityCache.Add(entity, domainEntity);

        return Mapper.Map(RepoDbSet.Add(domainEntity).Entity)!;
    }

    public virtual TKey GetIdFromDb(TDalEntity entity)
    {
        var domainEntity = _entityCache[entity];
        return domainEntity.Id;
    }

    public TDalEntity GetUpdatedEntityAfterSaveChanges(TDalEntity entity)
    {
        var updatedEntity = _entityCache[entity]!;
        var dalEntity = Mapper.Map(updatedEntity)!;

        return dalEntity;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        TDomainEntity domainEntity;
        if (!_entityCache.ContainsKey(entity))
        {
            domainEntity = Mapper.Map(entity)!;
            _entityCache.Add(entity, domainEntity);
        }
        else
        {
            domainEntity = _entityCache[entity];

            foreach (PropertyInfo each in domainEntity.GetType().GetFilteredProperties())
            {
                var entityProperty = entity.GetType().GetProperty(each.Name);
                if (entityProperty != null)
                {
                    each.SetValue(domainEntity, entityProperty?.GetValue(entity));
                }
            }
        }

        return Mapper.Map(RepoDbSet.Update(domainEntity).Entity)!;
    }

    public TDalEntity Remove(TDalEntity entity)
    {
        RepoDbContext.ChangeTracker.Clear();
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);

        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found!");
        }

        return Remove(entity);
    }

    public virtual TDalEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(a => a.Id.Equals(id)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(bool noTracking = true)
    {
        // Get all returns empty list, not list of nulls
        return CreateQuery(noTracking).ToList().Select(x => Mapper.Map(x)!);
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(a => a.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);

        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found!");
        }

        return Remove(entity);
    }
}