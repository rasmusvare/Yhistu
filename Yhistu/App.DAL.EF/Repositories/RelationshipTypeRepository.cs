using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class RelationshipTypeRepository :
    BaseEntityRepository<App.DAL.DTO.RelationshipType, App.Domain.RelationshipType, AppDbContext>,
    IRelationshipTypeRepository
{
    public RelationshipTypeRepository(AppDbContext dbContext, IMapper<RelationshipType, Domain.RelationshipType> mapper)
        : base(dbContext, mapper)
    {
    }
}