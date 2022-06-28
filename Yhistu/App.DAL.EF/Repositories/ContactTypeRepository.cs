using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContactTypeRepository :
    BaseEntityRepository<App.DAL.DTO.ContactType, App.Domain.ContactType, AppDbContext>, IContactTypeRepository
{
    public ContactTypeRepository(AppDbContext dbContext, IMapper<ContactType, Domain.ContactType> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<Guid> GetAddressTypeId()
    {
        var query = CreateQuery();

        var addressTypeId = await query.FirstAsync(c => c.Name == "Address");

        return addressTypeId.Id;
    }

    public async Task<Guid> GetEmailTypeId()
    {
        var query = CreateQuery();

        var emailType = await query.FirstAsync(c => c.Name == "Email");

        return emailType.Id;
    }

    public async Task<IEnumerable<ContactType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(a => a.AssociationId == associationId || a.AssociationId == null);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}