using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeasuringUnitMapper : BaseMapper<App.BLL.DTO.MeasuringUnit, App.DAL.DTO.MeasuringUnit>
{
    public MeasuringUnitMapper(IMapper mapper) : base(mapper)
    {
    }
}