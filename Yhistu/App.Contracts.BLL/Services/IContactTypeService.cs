using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IContactTypeService : IEntityService<App.BLL.DTO.ContactType>
{
    Task<Guid> GetAddressTypeId();
    Task<Guid> GetEmailTypeId();
    Task<IEnumerable<ContactType>> GetAllAsync(Guid associationId, bool noTracking = true);

}