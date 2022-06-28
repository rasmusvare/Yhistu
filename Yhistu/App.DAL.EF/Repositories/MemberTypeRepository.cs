using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.DAL.EF.Repositories;

public class MemberTypeRepository : BaseEntityRepository<App.DAL.DTO.MemberType, App.Domain.MemberType, AppDbContext>,
    IMemberTypeRepository
{
    public MemberTypeRepository(AppDbContext dbContext, IMapper<MemberType, Domain.MemberType> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<Guid?> GetAdminId()
    {
        //TODO: Doesn't really work

        var query = CreateQuery();
        var admin = await query.FirstOrDefaultAsync(m => m.IsAdministrator);

        return admin?.Id;
    }

    public async Task<IEnumerable<MemberType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery();

        query = query.Include(m=>m.Members).Where(a => a.AssociationId == associationId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> IsUsed(Guid memberTypeId)
    {
        var query = CreateQuery();

        var memberType = await query.Include(m=>m.Members).FirstOrDefaultAsync(a => a.Id == memberTypeId);

        return (memberType?.Members == null || memberType.Members.Count == 0);

    }
}