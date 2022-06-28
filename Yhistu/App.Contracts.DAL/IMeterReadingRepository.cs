using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeterReadingRepository: IEntityRepository<MeterReading>
{
    // Custom methods here
    Task<IEnumerable<MeterReading>> GetAllAsync(Guid meterId, bool noTracking = true);

    Task<MeterReading?> GetLastReading(Guid meterId);
}