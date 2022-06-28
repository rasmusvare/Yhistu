using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AssociationServiceMapper : BaseMapper<App.DAL.DTO.AssociationService, App.Domain.AssociationService>
{
    public AssociationServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}