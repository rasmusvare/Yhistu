using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MemberMapper : BaseMapper<App.BLL.DTO.Member, App.DAL.DTO.Member>
{
    public MemberMapper(IMapper mapper) : base(mapper)
    {
    }
}