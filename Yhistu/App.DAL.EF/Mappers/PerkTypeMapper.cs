using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PerkTypeMapper : BaseMapper<App.DAL.DTO.PerkType, App.Domain.PerkType>
{
    public PerkTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}