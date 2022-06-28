using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeterMapper : BaseMapper<App.BLL.DTO.Meter, App.DAL.DTO.Meter>
{
    public MeterMapper(IMapper mapper) : base(mapper)
    {
    }
}