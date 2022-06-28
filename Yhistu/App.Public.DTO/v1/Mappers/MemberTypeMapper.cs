using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MemberTypeMapper : BaseMapper<MemberType, App.BLL.DTO.MemberType>
{
    public MemberTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}