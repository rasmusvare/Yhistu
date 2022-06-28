using App.Contracts.DAL;
using App.DAL.DTO;
using AutoMapper;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ApartmentRepository : BaseEntityRepository<App.DAL.DTO.Apartment, App.Domain.Apartment, AppDbContext>,
    IApartmentRepository
{
    public ApartmentRepository(AppDbContext dbContext, IMapper<DAL.DTO.Apartment, Domain.Apartment> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<App.DAL.DTO.Apartment>> GetAllAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(a => a.ApartmentPersons)
            .SelectMany(a => a.ApartmentPersons!
                .Where(p => p.PersonId == userId))
            .Select(a => a.Apartment)!;

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<App.DAL.DTO.Apartment>> GetAllInBuildingAsync(Guid buildingId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Where(a => a.BuildingId == buildingId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public Task<IEnumerable<Apartment>> GetAllInAssociationAsync(Guid associaitionId, bool noTracking = true)
    {
        //TODO
        var repoDbSet = RepoDbContext.Set<Building>();
        var query = repoDbSet.AsQueryable();

        var apartment = query
            .Include(b => b.Apartments!)
            .ThenInclude(a => a.ApartmentPersons!)
            .ThenInclude(ap => ap.Person)
            .Where(b => b.AssociationId == associaitionId);

        // return (await apartment.Select(a => a.Apartments).ToListAsync())
        // .Select(x=>Mapper.Map(x));
        throw new NotImplementedException();
    }

    public async Task<Guid?> GetAssociationId(Guid apartmentId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var apartment = await query
            .Include(a => a.Building)
            .FirstOrDefaultAsync(a => a.Id == apartmentId);

        return apartment?.Building?.AssociationId;
    }


    public override async Task<App.DAL.DTO.Apartment?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var apartment = await query
            .Include(a => a.Building)
            .Include(a => a.Meters)
            .Include(a => a.ApartmentPersons)
            .FirstOrDefaultAsync(a => a.Id == id);

        return Mapper.Map(apartment);
    }

    public async Task<Apartment> GetForDeleteAsync(Guid id)
    {
        var query = CreateQuery();

        var apartment = await query
            .Include(a => a.Meters)!
            .ThenInclude(m => m.MeterReadings)
            .Include(a => a.ApartmentPersons)
            .FirstOrDefaultAsync(a => a.Id == id);
            

        return Mapper.Map(apartment)!;
    }
}