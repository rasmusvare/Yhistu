using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MemberMapper : BaseMapper<App.DAL.DTO.Member, App.Domain.Member>
{
    public MemberMapper(IMapper mapper) : base(mapper)
    {
    }
}