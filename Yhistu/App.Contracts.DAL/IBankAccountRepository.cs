using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IBankAccountRepository: IEntityRepository<BankAccount>, IBankAccountRepositoryCustom<BankAccount>
{
    // Custom methods here
    
}

public interface IBankAccountRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid associationId, bool noTracking = true);

}