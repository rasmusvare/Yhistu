using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IContactService : IEntityService<App.BLL.DTO.Contact>
{
    Task<string?> GetAssociationAddress(Guid id);
    Task<App.BLL.DTO.Contact?> GetBuildingAddress(Guid buildingId);
    
    Task<IEnumerable<App.BLL.DTO.Contact>> GetAllBuildingAsync(Guid buildingId);
    Task<IEnumerable<App.BLL.DTO.Contact>> GetAllAssociationAsync(Guid associationId);
    Task<IEnumerable<App.BLL.DTO.Contact>> GetAllPersonAsync(Guid personId);

    Task<App.BLL.DTO.Contact> AddAddress(Guid buildingId, Guid buildingAddress);

}