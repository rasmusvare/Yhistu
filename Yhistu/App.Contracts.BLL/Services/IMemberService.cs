using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMemberService: IEntityService<App.BLL.DTO.Member>, IMemberRepositoryCustom<App.BLL.DTO.Member>
{
    // Task<bool> IsMember(Guid personId, Guid associationId);
    // Task<bool> IsAdmin(Guid personId, Guid associationId);
    // Task<IEnumerable<Member>> GetBoardMembersAsync(Guid associationId, bool noTracking = true);
    // Task<IEnumerable<Member>> GetAllAsync(Guid associationId, bool noTracking = true);
    // Task<Member?> Find(Guid personId, Guid associationId);
    // public Member Add(Member entity);

}