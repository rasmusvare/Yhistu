using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAssociationRepository : IEntityRepository<Association>
{ 
    Task<IEnumerable<Association>> GetAllAsync(Guid userId, bool noTracking = true);

}