using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeterTypeService :
    BaseEntityService<App.BLL.DTO.MeterType, App.DAL.DTO.MeterType, IMeterTypeRepository>,
    IMeterTypeService
{
    public MeterTypeService(IMeterTypeRepository repository, IMapper<MeterType, DAL.DTO.MeterType> mapper) : base(
        repository, mapper)
    {
    }

    public async Task<IEnumerable<MeterType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);

    }
}