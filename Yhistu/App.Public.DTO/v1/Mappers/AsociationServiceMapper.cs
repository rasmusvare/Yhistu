using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class AsociationServiceMapper : BaseMapper<AssociationService, App.BLL.DTO.AssociationService>
{
    public AsociationServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}