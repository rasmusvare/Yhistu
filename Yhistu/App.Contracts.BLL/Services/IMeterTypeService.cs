using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMeterTypeService : IEntityService<App.BLL.DTO.MeterType>
{
    Task<IEnumerable<MeterType>> GetAllAsync(Guid associationId, bool noTracking = true);
}