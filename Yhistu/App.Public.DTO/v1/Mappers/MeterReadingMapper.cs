using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MeterReadingMapper : BaseMapper<MeterReading, App.BLL.DTO.MeterReading>
{
    public MeterReadingMapper(IMapper mapper) : base(mapper)
    {
    }
}