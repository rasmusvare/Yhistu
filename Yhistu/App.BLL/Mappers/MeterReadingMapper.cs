using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeterReadingMapper : BaseMapper<App.BLL.DTO.MeterReading, App.DAL.DTO.MeterReading>
{
    public MeterReadingMapper(IMapper mapper) : base(mapper)
    {
    }
}