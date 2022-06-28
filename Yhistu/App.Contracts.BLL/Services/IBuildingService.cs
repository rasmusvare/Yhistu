using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBuildingService: IEntityService<App.BLL.DTO.Building>
{
    Task<IEnumerable<Building>> GetAllAsync(Guid associationId, bool noTracking = true);
    // Task CalculateAreas(Guid buildingId);
    Task CalculateAreas(Building building);
}