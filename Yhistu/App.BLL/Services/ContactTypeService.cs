using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ContactTypeService :
    BaseEntityService<App.BLL.DTO.ContactType, App.DAL.DTO.ContactType, IContactTypeRepository>,
    IContactTypeService
{
    public ContactTypeService(IContactTypeRepository repository, IMapper<ContactType, DAL.DTO.ContactType> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<Guid> GetAddressTypeId()
    {
        return await Repository.GetAddressTypeId();
    }

    public async Task<Guid> GetEmailTypeId()
    {
        return await Repository.GetEmailTypeId();
    }

    public async Task<IEnumerable<ContactType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}