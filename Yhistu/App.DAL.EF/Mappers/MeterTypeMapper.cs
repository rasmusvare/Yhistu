using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeterTypeMapper : BaseMapper<App.DAL.DTO.MeterType, App.Domain.MeterType>
{
    public MeterTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}