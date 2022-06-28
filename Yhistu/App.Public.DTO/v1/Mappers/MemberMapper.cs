using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MemberMapper:BaseMapper<Member, App.BLL.DTO.Member>
{
    public MemberMapper(IMapper mapper) : base(mapper)
    {
    }
}