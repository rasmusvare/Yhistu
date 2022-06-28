using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ApartmentService : BaseEntityService<App.BLL.DTO.Apartment, App.DAL.DTO.Apartment, IApartmentRepository>,
    IApartmentService
{
    public ApartmentService(IApartmentRepository repository, IMapper<Apartment, DAL.DTO.Apartment> mapper) : base(
        repository, mapper)
    {
    }
    public async Task<IEnumerable<App.BLL.DTO.Apartment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x=> Mapper.Map(x)!);
    }

    public async Task<Guid?> GetAssociationId(Guid apartmentId, bool noTracking = true)
    {
        return await Repository.GetAssociationId(apartmentId, noTracking);
    }

    public async Task<IEnumerable<Apartment>> GetAllInBuildingAsync(Guid buildingId, bool noTracking = true)
    {
        return (await Repository.GetAllInBuildingAsync(buildingId, noTracking)).Select(x=>Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Apartment>> GetAllInAssociationAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllInAssociationAsync(associationId, noTracking)).Select(x=>Mapper.Map(x)!);
    }

    public async Task<Apartment> GetForDeleteAsync(Guid id)
    {
        return Mapper.Map(await Repository.GetForDeleteAsync(id))!;
    }
}