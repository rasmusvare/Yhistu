using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeasuringUnitMapper : BaseMapper<App.DAL.DTO.MeasuringUnit, App.Domain.MeasuringUnit>
{
    public MeasuringUnitMapper(IMapper mapper) : base(mapper)
    {
    }
}