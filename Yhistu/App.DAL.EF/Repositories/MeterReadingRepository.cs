using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MeterReadingRepository :
    BaseEntityRepository<App.DAL.DTO.MeterReading, App.Domain.MeterReading, AppDbContext>, IMeterReadingRepository
{
    public MeterReadingRepository(AppDbContext dbContext, IMapper<MeterReading, Domain.MeterReading> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<MeterReading>> GetAllAsync(Guid meterId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Where(m => m.MeterId == meterId)
            .OrderByDescending(m => m.Date).ThenByDescending(m=>m.Value);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<MeterReading?> GetLastReading(Guid meterId)
    {
        var query = CreateQuery();

        var lastReading = await query.Where(m => m.MeterId == meterId).OrderByDescending(m => m.Date).ThenByDescending(m=>m.Value).FirstOrDefaultAsync();

        return Mapper.Map(lastReading)!;
    }
}