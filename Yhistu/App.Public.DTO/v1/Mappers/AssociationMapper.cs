using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class AssociationMapper : BaseMapper<Association, App.BLL.DTO.Association>
{
    public AssociationMapper(IMapper mapper) : base(mapper)
    {
    }
}