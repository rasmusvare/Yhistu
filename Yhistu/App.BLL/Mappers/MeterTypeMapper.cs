using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MeterTypeMapper : BaseMapper<App.BLL.DTO.MeterType, App.DAL.DTO.MeterType>
{
    public MeterTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}