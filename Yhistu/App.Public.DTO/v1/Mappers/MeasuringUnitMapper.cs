using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MeasuringUnitMapper : BaseMapper<MeasuringUnit, App.BLL.DTO.MeasuringUnit>
{
    public MeasuringUnitMapper(IMapper mapper) : base(mapper)
    {
    }
}