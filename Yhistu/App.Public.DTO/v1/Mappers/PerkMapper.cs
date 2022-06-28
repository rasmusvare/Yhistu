using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class PerkMapper : BaseMapper<Perk, App.BLL.DTO.Perk>
{
    public PerkMapper(IMapper mapper) : base(mapper)
    {
    }
}