using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IApartmentRepository : IEntityRepository<Apartment>, IApartmentRepositoryCustom<Apartment>
{
    // Custom methods here
    Task<Guid?> GetAssociationId(Guid apartmentId, bool noTracking = true);
}

public interface IApartmentRepositoryCustom<TEntity>
{   
    Task<IEnumerable<TEntity>> GetAllAsync(Guid userId, bool noTracking = true);

    Task<IEnumerable<TEntity>> GetAllInBuildingAsync(Guid buildingId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllInAssociationAsync(Guid associationId, bool noTracking = true);

    public Task<TEntity> GetForDeleteAsync(Guid id);

}