using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBankAccountService : IEntityService<App.BLL.DTO.BankAccount>, IBankAccountRepositoryCustom<App.BLL.DTO.BankAccount>
{
}