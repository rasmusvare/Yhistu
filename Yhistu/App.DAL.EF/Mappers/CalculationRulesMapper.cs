using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CalculationRulesMapper : BaseMapper<App.DAL.DTO.CalculationRules, App.Domain.CalculationRules>
{
    public CalculationRulesMapper(IMapper mapper) : base(mapper)
    {
    }
}