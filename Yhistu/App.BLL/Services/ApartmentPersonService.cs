using System.Dynamic;
using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ApartmentPersonService : BaseEntityService<App.BLL.DTO.ApartmentPerson, App.DAL.DTO.ApartmentPerson,
        IApartmentPersonRepository>,
    IApartmentPersonService
{
    public ApartmentPersonService(IApartmentPersonRepository repository,
        IMapper<ApartmentPerson, DAL.DTO.ApartmentPerson> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<ApartmentPerson>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<ApartmentPerson>> GetAllApartmentAsync(Guid apartmentId, bool noTracking = true)
    {
        return (await Repository.GetAllApartmentAsync(apartmentId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> HasConnection(Guid personId, Guid apartmentId)
    {
        return await Repository.HasConnection(personId, apartmentId);
    }

    public Task<bool> ShareApartment(Guid personId, Guid otherPersonId)
    {
        return Repository.ShareApartment(personId, otherPersonId);
    }

    public async Task<ApartmentPerson?> FirstOrDefaultAsync(ApartmentPerson apartmentPerson)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(Mapper.Map(apartmentPerson)!));
    }
}