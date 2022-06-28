using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CalculationRulesMapper : BaseMapper<App.BLL.DTO.CalculationRules, App.DAL.DTO.CalculationRules>
{
    public CalculationRulesMapper(IMapper mapper) : base(mapper)
    {
    }
}