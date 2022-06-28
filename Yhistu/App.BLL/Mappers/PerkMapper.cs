using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PerkMapper : BaseMapper<App.BLL.DTO.Perk, App.DAL.DTO.Perk>
{
    public PerkMapper(IMapper mapper) : base(mapper)
    {
    }
}