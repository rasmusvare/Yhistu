using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMeasuringUnitService : IEntityService<App.BLL.DTO.MeasuringUnit>
{
    Task<IEnumerable<MeasuringUnit>> GetAllAsync(Guid associationId, bool noTracking = true);

}