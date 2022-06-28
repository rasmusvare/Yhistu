using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeasuringUnitService :
    BaseEntityService<App.BLL.DTO.MeasuringUnit, App.DAL.DTO.MeasuringUnit, IMeasuringUnitRepository>,
    IMeasuringUnitService
{
    public MeasuringUnitService(IMeasuringUnitRepository repository,
        IMapper<MeasuringUnit, DAL.DTO.MeasuringUnit> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<MeasuringUnit>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}