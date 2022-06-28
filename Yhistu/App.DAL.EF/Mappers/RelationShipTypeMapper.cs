using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RelationShipTypeMapper : BaseMapper<App.DAL.DTO.RelationshipType, App.Domain.RelationshipType>
{
    public RelationShipTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}