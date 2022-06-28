using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AssociationMapper : BaseMapper<App.DAL.DTO.Association, App.Domain.Association>
{
    public AssociationMapper(IMapper mapper) : base(mapper)
    {
    }
}