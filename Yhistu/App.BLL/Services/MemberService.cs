using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MemberService : BaseEntityService<App.BLL.DTO.Member, App.DAL.DTO.Member, IMemberRepository>,
    IMemberService
{
    public MemberService(IMemberRepository repository, IMapper<Member, DAL.DTO.Member> mapper) : base(repository,
        mapper)
    {
    }

    public async Task<bool> IsMember(Guid personId, Guid associationId)
    {
        return await Repository.IsMember(personId, associationId);
    }

    public async Task<bool> IsAdmin(Guid personId, Guid associationId)
    {
        return await Repository.IsAdmin(personId, associationId);
    }

    public async Task<IEnumerable<Member>> GetBoardMembersAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetBoardMembersAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Member>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<Member?> Find(Guid personId, Guid associationId)
    {
        return Mapper.Map(await Repository.Find(personId, associationId));
    }

    public async Task<IEnumerable<Member>> GetAllForDeleteAsync(Guid associationId, bool noTracking)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<Member?> FirstOrDefaultAsync(Member member)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(Mapper.Map(member)!));
    }
}