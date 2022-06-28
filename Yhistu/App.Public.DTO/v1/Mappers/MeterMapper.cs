using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MeterMapper : BaseMapper<Meter, App.BLL.DTO.Meter>
{
    public MeterMapper(IMapper mapper) : base(mapper)
    {
    }
}