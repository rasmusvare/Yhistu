using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BankAccountMapper : BaseMapper<App.BLL.DTO.BankAccount, App.DAL.DTO.BankAccount>
{
    public BankAccountMapper(IMapper mapper) : base(mapper)
    {
    }
}