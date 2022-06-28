using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeterReadingService :
    BaseEntityService<App.BLL.DTO.MeterReading, App.DAL.DTO.MeterReading, IMeterReadingRepository>,
    IMeterReadingService
{
    public MeterReadingService(IMeterReadingRepository repository, IMapper<MeterReading, DAL.DTO.MeterReading> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<MeterReading>> GetAllAsync(Guid meterId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(meterId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> Validate(MeterReading bllEntity)
    {
        var lastReading = await Repository.GetLastReading(bllEntity.MeterId);
        
        if (lastReading == null)
        {
            return true;
        }

        return lastReading.Value < bllEntity.Value;
    }
}