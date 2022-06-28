using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PerkTypeMapper : BaseMapper<App.BLL.DTO.PerkType, App.DAL.DTO.PerkType>
{
    public PerkTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}