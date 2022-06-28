using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AssociationRepository : BaseEntityRepository<App.DAL.DTO.Association, Domain.Association, AppDbContext>,
    IAssociationRepository
{
    public AssociationRepository(AppDbContext dbContext,
        IMapper<App.DAL.DTO.Association, App.Domain.Association> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<Association>> GetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(b=>b.BankAccounts);    
        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Association>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(m => m.Members)
            .Where(a => a.Members!.Any(m => m.PersonId == userId));

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}