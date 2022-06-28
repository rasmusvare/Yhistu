using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MemberTypeMapper : BaseMapper<App.BLL.DTO.MemberType, App.DAL.DTO.MemberType>
{
    public MemberTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}