using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBuildingRepository : IEntityRepository<Building>
{
    // Custom methods here

    Task<IEnumerable<Building>> GetAllAsync(Guid associationId, bool noTracking = true);
    Task<Building?> GetWithApartmentsAsync(Guid buildingId, bool noTracking = true);
}