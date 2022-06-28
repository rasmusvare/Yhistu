using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IApartmentService : IEntityService<App.BLL.DTO.Apartment>, IApartmentRepositoryCustom<App.BLL.DTO.Apartment>
{
    // Task<IEnumerable<App.BLL.DTO.Apartment>> GetAllAsync(Guid userId, bool noTracking = true);

    Task<Guid?> GetAssociationId(Guid apartmentId, bool noTracking = true);
}