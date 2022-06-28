using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMeterService : IEntityService<App.BLL.DTO.Meter>
{
    Task<IEnumerable<Meter>?> GetAllAsync(Guid id, string type, bool noTracking = true);
    Task<Guid?> GetAssociationId(Guid meterId, bool noTracking = true);
}