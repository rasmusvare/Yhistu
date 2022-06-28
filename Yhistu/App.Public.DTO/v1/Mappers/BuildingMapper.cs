using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class BuildingMapper : BaseMapper<Building, App.BLL.DTO.Building>
{
    public BuildingMapper(IMapper mapper) : base(mapper)
    {
    }
}