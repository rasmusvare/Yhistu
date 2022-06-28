using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMemberRepository : IEntityRepository<Member>, IMemberRepositoryCustom<Member>
{
}

public interface IMemberRepositoryCustom<TEntity>
{
    // Custom methods here
    
    Task<bool> IsMember(Guid personId, Guid associationId);
    Task<bool> IsAdmin(Guid personId, Guid associationId);
    Task<IEnumerable<TEntity>> GetBoardMembersAsync(Guid associationId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(Guid associationId, bool noTracking = true);
    Task<TEntity?> Find(Guid personId, Guid associationId);
    Task<IEnumerable<TEntity>> GetAllForDeleteAsync(Guid associationId, bool noTracking = true);
    Task<TEntity?> FirstOrDefaultAsync(TEntity member);

}