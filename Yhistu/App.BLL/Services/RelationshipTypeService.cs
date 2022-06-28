using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class RelationshipTypeService : BaseEntityService<App.BLL.DTO.RelationshipType, App.DAL.DTO.RelationshipType,
        IRelationshipTypeRepository>,
    IRelationshipTypeService
{
    public RelationshipTypeService(IRelationshipTypeRepository repository,
        IMapper<RelationshipType, DAL.DTO.RelationshipType> mapper) : base(repository, mapper)
    {
    }
}