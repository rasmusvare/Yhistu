using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IContactTypeRepository: IEntityRepository<ContactType>
{
    // Custom methods here
    Task<Guid> GetAddressTypeId();
    Task<Guid> GetEmailTypeId();
    Task<IEnumerable<ContactType>> GetAllAsync(Guid associationId, bool noTracking = true);

}
