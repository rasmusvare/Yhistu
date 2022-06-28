using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class AssociationServiceService : BaseEntityService<App.BLL.DTO.AssociationService,
        App.DAL.DTO.AssociationService, IAssociationServiceRepository>,
    IAssociationServiceService

{
    public AssociationServiceService(IAssociationServiceRepository repository,
        IMapper<DTO.AssociationService, DAL.DTO.AssociationService> mapper) : base(repository, mapper)
    {
    }
}