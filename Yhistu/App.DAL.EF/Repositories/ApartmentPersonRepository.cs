using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartmentPersonRepository :
    BaseEntityRepository<App.DAL.DTO.ApartmentPerson, App.Domain.ApartmentPerson, AppDbContext>,
    IApartmentPersonRepository
{
    public ApartmentPersonRepository(AppDbContext dbContext, IMapper<ApartmentPerson, Domain.ApartmentPerson> mapper) :
        base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<ApartmentPerson>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(ap => ap.PersonId == userId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<ApartmentPerson>> GetAllApartmentAsync(Guid apartmentId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(ap => ap.Person)
            .Where(ap => ap.ApartmentId == apartmentId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<bool> HasConnection(Guid personId, Guid apartmentId)
    {
        var query = CreateQuery();

        query = query.Where(ap => ap.ApartmentId == apartmentId);

        return await query.AnyAsync(ap => ap.PersonId == personId);
    }

    public async Task<bool> ShareApartment(Guid personId, Guid otherPersonId)
    {
        var query = CreateQuery();

        var hasConnection = await query
            .Where(ap => ap.PersonId == personId)
            .Select(ap => ap.Apartment!)
            .Include(a => a.ApartmentPersons)
            .Select(a => a.ApartmentPersons!)
            .AnyAsync(ap => ap.Any(p => p.PersonId == otherPersonId));

        return hasConnection;
    }

    public async Task<ApartmentPerson?> FirstOrDefaultAsync(ApartmentPerson apartmentPerson)
    {
        var query = CreateQuery();

        var personDb = await query
            .FirstOrDefaultAsync(ap => ap.PersonId == apartmentPerson.PersonId
                                       && ap.ApartmentId == apartmentPerson.ApartmentId);

        return Mapper.Map(personDb);
    }
}