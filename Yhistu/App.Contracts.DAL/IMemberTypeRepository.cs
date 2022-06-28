using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMemberTypeRepository: IEntityRepository<MemberType>, IMemberTypeRepositoryCustom<MemberType>
{
    // Custom methods here
    Task<IEnumerable<MemberType>> GetAllAsync(Guid associationId, bool noTracking = true);
}

public interface IMemberTypeRepositoryCustom<TEntity>
{
    Task<Guid?> GetAdminId();
    Task<bool> IsUsed(Guid id);

}