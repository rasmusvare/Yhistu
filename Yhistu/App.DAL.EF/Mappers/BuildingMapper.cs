using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BuildingMapper : BaseMapper<App.DAL.DTO.Building, App.Domain.Building>
{
    public BuildingMapper(IMapper mapper) : base(mapper)
    {
    }
}