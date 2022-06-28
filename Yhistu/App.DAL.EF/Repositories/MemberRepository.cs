using App.Contracts.DAL;
using App.DAL.DTO;
using AutoMapper.QueryableExtensions.Impl;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MemberRepository : BaseEntityRepository<App.DAL.DTO.Member, App.Domain.Member, AppDbContext>,
    IMemberRepository
{
    public MemberRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Member, App.Domain.Member> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<bool> IsMember(Guid personId, Guid associationId)
    {
        return await RepoDbSet.AnyAsync(m => m.PersonId == personId
                                             && m.AssociationId == associationId);
    }

    public async Task<bool> IsAdmin(Guid personId, Guid associationId)
    {
        var query = CreateQuery();

        var isAdmin = await query
            .Where(m => m.PersonId == personId && m.AssociationId == associationId)
            .Include(m => m.MemberType)
            .AnyAsync(m => m.MemberType!.IsAdministrator && m.ViewAsRegularUser == false);

        // return await RepoDbSet.Include(m => m.MemberType).AnyAsync(m =>
        //     m.PersonId == personId 
        //     && m.AssociationId == associationId 
        //     && m.MemberType!.IsAdministrator);
        return isAdmin;
    }

    public async Task<IEnumerable<Member>> GetBoardMembersAsync(Guid associationId, bool noTracking)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(m => m.MemberType)
            .Where(m => m.AssociationId == associationId)
            .Where(m => m.MemberType!.IsAdministrator == true || m.MemberType.IsMemberOfBoard == true ||
                        m.MemberType.IsManager == true)
            .Include(m => m.Person);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Member>> GetAllAsync(Guid associationId, bool noTracking)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(m => m.MemberType)
            .Where(m => m.AssociationId == associationId)
            .Include(m => m.Person);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Member>> GetAllForDeleteAsync(Guid associationId, bool noTracking)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Where(m => m.AssociationId == associationId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<Member?> FirstOrDefaultAsync(Member member)
    {
        var query = CreateQuery();

        var personDb = await query
            .FirstOrDefaultAsync(m => m.PersonId == member.PersonId
                                      && m.AssociationId == member.AssociationId);

        return Mapper.Map(personDb);
    }

    public async Task<Member?> Find(Guid personId, Guid associationId)
    {
        var query = CreateQuery();

        var member = await query.Include(p => p.MemberType)
            .FirstOrDefaultAsync(m => m.PersonId == personId && m.AssociationId == associationId);

        return Mapper.Map(member);
    }
}