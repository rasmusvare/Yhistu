using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class CalculationRulesRepository :
    BaseEntityRepository<App.DAL.DTO.CalculationRules, App.Domain.CalculationRules, AppDbContext>,
    ICalculationRulesRepository
{
    public CalculationRulesRepository(AppDbContext dbContext, IMapper<CalculationRules, Domain.CalculationRules> mapper)
        : base(dbContext, mapper)
    {
    }
}