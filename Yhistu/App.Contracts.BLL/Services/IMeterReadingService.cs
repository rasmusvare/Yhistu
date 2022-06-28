using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMeterReadingService : IEntityService<App.BLL.DTO.MeterReading>
{
    Task<IEnumerable<MeterReading>> GetAllAsync(Guid meterId, bool noTracking = true);
    Task<bool> Validate(MeterReading bllEntity);
}