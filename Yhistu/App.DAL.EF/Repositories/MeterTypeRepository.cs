using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MeterTypeRepository : BaseEntityRepository<App.DAL.DTO.MeterType, App.Domain.MeterType, AppDbContext>,
    IMeterTypeRepository
{
    public MeterTypeRepository(AppDbContext dbContext, IMapper<MeterType, Domain.MeterType> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<MeterType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Include(m=>m.MeasuringUnit).Where(a => a.AssociationId == associationId || a.AssociationId == null);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}