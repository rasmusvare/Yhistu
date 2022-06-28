using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BankAccountService :
    BaseEntityService<App.BLL.DTO.BankAccount, App.DAL.DTO.BankAccount, IBankAccountRepository>,
    IBankAccountService
{
    public BankAccountService(IBankAccountRepository repository, IMapper<BankAccount, DAL.DTO.BankAccount> mapper) :
        base(repository, mapper)
    {
    }


    public async Task<IEnumerable<BankAccount>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x=> Mapper.Map(x)!);
    }
}