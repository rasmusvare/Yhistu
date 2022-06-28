using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BankAccountMapper : BaseMapper<App.DAL.DTO.BankAccount, App.Domain.BankAccount>
{
    public BankAccountMapper(IMapper mapper) : base(mapper)
    {
    }
}