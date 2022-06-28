using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class CalculationRulesService : BaseEntityService<App.BLL.DTO.CalculationRules, App.DAL.DTO.CalculationRules,
        ICalculationRulesRepository>,
    ICalculationRulesService
{
    public CalculationRulesService(ICalculationRulesRepository repository,
        IMapper<CalculationRules, DAL.DTO.CalculationRules> mapper) : base(repository, mapper)
    {
    }
}