using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeterReadingMapper : BaseMapper<App.DAL.DTO.MeterReading, App.Domain.MeterReading>
{
    public MeterReadingMapper(IMapper mapper) : base(mapper)
    {
    }
}