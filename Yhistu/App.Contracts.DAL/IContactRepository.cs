using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IContactRepository: IEntityRepository<Contact>
{
    // Custom methods here
    Task<string?> GetAssociationAddress(Guid id);
    Task<Contact?> GetBuildingAddress(Guid buildingId);
    
    Task<IEnumerable<Contact>> GetAllBuildingAsync(Guid buildingId);
    Task<IEnumerable<Contact>> GetAllAssociationAsync(Guid buildingId);
    Task<IEnumerable<Contact>> GetAllPersonAsync(Guid buildingId);

}
