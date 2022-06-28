using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class AssociationServiceMapper : BaseMapper<App.BLL.DTO.AssociationService, App.DAL.DTO.AssociationService>
{
    public AssociationServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}