using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MeasuringUnitRepository :
    BaseEntityRepository<App.DAL.DTO.MeasuringUnit, App.Domain.MeasuringUnit, AppDbContext>, IMeasuringUnitRepository
{
    public MeasuringUnitRepository(AppDbContext dbContext, IMapper<MeasuringUnit, Domain.MeasuringUnit> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<MeasuringUnit>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(a => a.AssociationId == associationId || a.AssociationId == null);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}