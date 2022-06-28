using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPersonRepository : IEntityRepository<Person>, IPersonRepositoryCustom<Person>
{
}

public interface IPersonRepositoryCustom<TEntity>
{
    Task<Guid> GetMainPersonId(Guid userId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(Guid appUserId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAssociationAsync(Guid associationId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetBoardMembersAsync(Guid associationId, bool noTracking = true);
    Task<TEntity?> FirstOrDefaultAsync(TEntity apartmentPerson);
    Task<TEntity?> FindByEmail(string email);
}