using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class RelationShipTypeMapper : BaseMapper<App.BLL.DTO.RelationshipType, App.DAL.DTO.RelationshipType>
{
    public RelationShipTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}