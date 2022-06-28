using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MemberTypeMapper : BaseMapper<App.DAL.DTO.MemberType, App.Domain.MemberType>
{
    public MemberTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}