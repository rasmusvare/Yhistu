using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IPersonService : IEntityService<App.BLL.DTO.Person>, IPersonRepositoryCustom<App.BLL.DTO.Person>
{
    // Task<IEnumerable<Person>> GetAllAsync(Guid personId, bool noTracking = true);
    // Task<IEnumerable<Person>> GetAllAssociationAsync(Guid associationId);
}

