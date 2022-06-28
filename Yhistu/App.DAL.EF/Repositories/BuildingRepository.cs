using System.Diagnostics.CodeAnalysis;
using App.Contracts.DAL;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class BuildingRepository : BaseEntityRepository<App.DAL.DTO.Building, Domain.Building, AppDbContext>,
    IBuildingRepository
{
    public BuildingRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Building, App.Domain.Building> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Building>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(a => a.AssociationId == associationId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<Building?> GetWithApartmentsAsync(Guid buildingId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var building = await query
            .Include(b => b.Apartments)
            .FirstOrDefaultAsync(b => b.Id == buildingId);

        RepoDbContext.ChangeTracker.Clear();

        return Mapper.Map(building);
    }
}