using System.Dynamic;
using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContactRepository : BaseEntityRepository<App.DAL.DTO.Contact, App.Domain.Contact, AppDbContext>,
    IContactRepository
{
    public ContactRepository(AppDbContext dbContext, IMapper<Contact, Domain.Contact> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<string?> GetAssociationAddress(Guid id)
    {
        var query = CreateQuery();

        var address = await query.Include(c => c.ContactType)
            .FirstOrDefaultAsync(c => c.ContactType!.Name == "Address" && c.AssociationId == id);

        return address?.Value;
    }
    
    public async Task<Contact?> GetBuildingAddress(Guid buildingId)
    {
        var query = CreateQuery();

        var address = await query.Include(c => c.ContactType)
            .FirstOrDefaultAsync(c => c.ContactType!.Name == "Address" && c.BuildingId == buildingId);

        return Mapper.Map(address);
    }

    public async Task<IEnumerable<Contact>> GetAllBuildingAsync(Guid buildingId)
    {
        var query = CreateQuery();

        query = query.Where(c => c.BuildingId == buildingId).Include(c => c.ContactType);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Contact>> GetAllAssociationAsync(Guid associationId)
    {
        var query = CreateQuery();

        query = query.Where(c => c.AssociationId == associationId).Include(c => c.ContactType);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<Contact>> GetAllPersonAsync(Guid personId)
    {
        var query = CreateQuery();

        query = query.Where(c => c.PersonId == personId).Include(c => c.ContactType);

        return (await query.ToListAsync()).Select(x => Mapper.Map(x)!);
    }
}