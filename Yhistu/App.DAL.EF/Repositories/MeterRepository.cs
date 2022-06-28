using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MeterRepository : BaseEntityRepository<App.DAL.DTO.Meter, App.Domain.Meter, AppDbContext>, IMeterRepository
{
    public MeterRepository(AppDbContext dbContext, IMapper<Meter, Domain.Meter> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Meter>> GetAllAsync(Guid buildingId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(m => m.MeterType)
            .ThenInclude(t=>t!.MeasuringUnit)
            // .Include(m => m.MeterReadings)
            .Where(m => m.BuildingId == buildingId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Meter>> GetAllApartmentAsync(Guid apartmentId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(m => m.MeterType)
            .ThenInclude(t=>t!.MeasuringUnit)
            // .Include(m => m.MeterReadings)
            .Where(m => m.ApartmentId == apartmentId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Meter>> GetAllBuildingAsync(Guid buildingId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query
            .Include(m => m.MeterType)
            .ThenInclude(t=>t!.MeasuringUnit)
            // .Include(m => m.MeterReadings)
            .Where(m => m.BuildingId == buildingId && m.ApartmentId == null);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<Guid?> GetAssociationId(Guid meterId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var meter = await query
            .Include(m => m.Building)
            // .Include(m => m.Apartment)
            // .ThenInclude(a=>a!.Building)
            .FirstOrDefaultAsync(m => m.Id == meterId);

        Guid? associationId = null;
        if (meter?.Building != null)
        {
            associationId = meter.Building.AssociationId;
        } else if (meter?.Apartment?.Building != null)
        {
            associationId = meter.Apartment.Building.AssociationId;
        }

        return associationId;

    }

    public override async Task<Meter> RemoveAsync(Guid id)
    {
        var query = CreateQuery();
        var meter = await query
            .Include(m => m.MeterReadings)
            .Include(m => m.InvoiceRows)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (meter?.MeterReadings != null)
        {
            foreach (var readings in meter.MeterReadings)
            {
                await base.RemoveAsync(readings.Id);
            }
        }

        if (meter?.InvoiceRows != null)
        {
            foreach (var invoiceRow in meter.InvoiceRows)
            {
                await base.RemoveAsync(invoiceRow.Id);
            }
        }

        return await base.RemoveAsync(id);
    }
}