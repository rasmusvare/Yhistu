using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PerkTypeRepository : BaseEntityRepository<App.DAL.DTO.PerkType, App.Domain.PerkType, AppDbContext>,
    IPerkTypeRepository
{
    public PerkTypeRepository(AppDbContext dbContext, IMapper<PerkType, Domain.PerkType> mapper) : base(dbContext,
        mapper)
    {
    }
}