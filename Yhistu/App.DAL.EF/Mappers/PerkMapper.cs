using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PerkMapper : BaseMapper<App.DAL.DTO.Perk, App.Domain.Perk>
{
    public PerkMapper(IMapper mapper) : base(mapper)
    {
    }
}