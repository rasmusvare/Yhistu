using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeterRepository : IEntityRepository<Meter>
{
    // Custom methods here
    Task<IEnumerable<Meter>> GetAllAsync(Guid buildingId, bool noTracking = true);
    Task<IEnumerable<Meter>> GetAllApartmentAsync(Guid apartmentId, bool noTracking = true);
    Task<IEnumerable<Meter>> GetAllBuildingAsync(Guid buildingId, bool noTracking = true);
    Task<Guid?> GetAssociationId(Guid meterId, bool noTracking = true);
}