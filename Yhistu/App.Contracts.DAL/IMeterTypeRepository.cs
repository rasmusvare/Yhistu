using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMeterTypeRepository: IEntityRepository<MeterType>
{
    // Custom methods here
    Task<IEnumerable<MeterType>> GetAllAsync(Guid associationId, bool noTracking = true);

}