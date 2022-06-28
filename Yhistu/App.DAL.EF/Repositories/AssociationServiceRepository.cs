using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AssociationServiceRepository :
    BaseEntityRepository<App.DAL.DTO.AssociationService, App.Domain.AssociationService, AppDbContext>,
    IAssociationServiceRepository
{
    public AssociationServiceRepository(AppDbContext dbContext,
        IMapper<AssociationService, Domain.AssociationService> mapper) : base(dbContext, mapper)
    {
    }
}