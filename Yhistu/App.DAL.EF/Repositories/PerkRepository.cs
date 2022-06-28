using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PerkRepository : BaseEntityRepository<App.DAL.DTO.Perk, App.Domain.Perk, AppDbContext>, IPerkRepository
{
    public PerkRepository(AppDbContext dbContext, IMapper<Perk, Domain.Perk> mapper) : base(dbContext, mapper)
    {
    }
}