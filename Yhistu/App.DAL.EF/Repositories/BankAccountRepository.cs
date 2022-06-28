using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BankAccountRepository :
    BaseEntityRepository<App.DAL.DTO.BankAccount, App.Domain.BankAccount, AppDbContext>, IBankAccountRepository
{
    public BankAccountRepository(AppDbContext dbContext, IMapper<BankAccount, Domain.BankAccount> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<BankAccount>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Where(a => a.AssociationId == associationId);
            
        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}