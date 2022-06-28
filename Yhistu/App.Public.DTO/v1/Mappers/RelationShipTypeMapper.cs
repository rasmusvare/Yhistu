using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class RelationShipTypeMapper : BaseMapper<RelationshipType, App.BLL.DTO.RelationshipType>
{
    public RelationShipTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}