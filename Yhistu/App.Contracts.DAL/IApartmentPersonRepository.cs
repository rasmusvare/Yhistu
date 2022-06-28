using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartmentPersonRepository : IEntityRepository<ApartmentPerson>,
    IApartmentPersonRepositoryCustom<ApartmentPerson>
{
    // Custom methods here
}

public interface IApartmentPersonRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllApartmentAsync(Guid apartmentId, bool noTracking = true);
    Task<bool> HasConnection(Guid personId, Guid apartmentId);
    Task<bool> ShareApartment(Guid personId, Guid otherPersonId);
    Task<TEntity?> FirstOrDefaultAsync(TEntity person);
}