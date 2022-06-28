using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAssociationService: IEntityService<App.BLL.DTO.Association>
{
    Task<IEnumerable<Association>> GetAllAsync(Guid userId, bool noTracking = true);

}