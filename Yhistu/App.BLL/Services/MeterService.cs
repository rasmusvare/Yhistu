using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MeterService : BaseEntityService<App.BLL.DTO.Meter, App.DAL.DTO.Meter, IMeterRepository>,
    IMeterService
{
    public MeterService(IMeterRepository repository, IMapper<Meter, DAL.DTO.Meter> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Meter>?> GetAllAsync(Guid id, string type, bool noTracking = true)
    {
        return type switch
        {
            "apartment" => (await Repository.GetAllApartmentAsync(id, noTracking)).Select(x => Mapper.Map(x)!),
            "building" => (await Repository.GetAllBuildingAsync(id, noTracking)).Select(x => Mapper.Map(x)!),
            "all" => (await Repository.GetAllAsync(id, noTracking)).Select(x => Mapper.Map(x)!),
            _ => null
        };
    }

    public async Task<Guid?> GetAssociationId(Guid meterId, bool noTracking = true)
    {
        return await Repository.GetAssociationId(meterId, noTracking);
    }
}