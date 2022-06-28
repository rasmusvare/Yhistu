using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeasuringUnitRepository: IEntityRepository<MeasuringUnit>
{
    // Custom methods here
    Task<IEnumerable<MeasuringUnit>> GetAllAsync(Guid associationId, bool noTracking = true);

}