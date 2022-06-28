using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ContactService : BaseEntityService<App.BLL.DTO.Contact, App.DAL.DTO.Contact, IContactRepository>,
    IContactService
{
    public ContactService(IContactRepository repository, IMapper<Contact, DAL.DTO.Contact> mapper) : base(repository,
        mapper)
    {
    }

    public async Task<string?> GetAssociationAddress(Guid id)
    {
        return await Repository.GetAssociationAddress(id);
    }

    public async Task<Contact?> GetBuildingAddress(Guid buildingId)
    {
        return Mapper.Map(await Repository.GetBuildingAddress(buildingId));
    }

    public async Task<IEnumerable<Contact>> GetAllBuildingAsync(Guid buildingId)
    {
        return (await Repository.GetAllBuildingAsync(buildingId)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Contact>> GetAllAssociationAsync(Guid associationId)
    {
        return (await Repository.GetAllAssociationAsync(associationId)).Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<Contact>> GetAllPersonAsync(Guid personId)
    {
        return (await Repository.GetAllPersonAsync(personId)).Select(x => Mapper.Map(x)!);
    }

    public Task<Contact> AddAddress(Guid buildingId, Guid buildingAddress)
    {
        throw new NotImplementedException();
    }
}