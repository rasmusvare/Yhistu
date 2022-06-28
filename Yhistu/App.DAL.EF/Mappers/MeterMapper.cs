using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MeterMapper : BaseMapper<App.DAL.DTO.Meter, App.Domain.Meter>
{
    public MeterMapper(IMapper mapper) : base(mapper)
    {
    }
}