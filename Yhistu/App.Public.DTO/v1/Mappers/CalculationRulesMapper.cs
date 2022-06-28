using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class CalculationRulesMapper : BaseMapper<CalculationRules, App.BLL.DTO.CalculationRules>
{
    public CalculationRulesMapper(IMapper mapper) : base(mapper)
    {
    }
}