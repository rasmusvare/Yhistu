using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class BankAccountMapper : BaseMapper<BankAccount, App.BLL.DTO.BankAccount>
{
    public BankAccountMapper(IMapper mapper) : base(mapper)
    {
    }
}