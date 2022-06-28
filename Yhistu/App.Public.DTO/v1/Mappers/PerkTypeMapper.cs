using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class PerkTypeMapper : BaseMapper<PerkType, App.BLL.DTO.PerkType>
{
    public PerkTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}