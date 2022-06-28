using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MeterTypeMapper : BaseMapper<MeterType, App.BLL.DTO.MeterType>
{
    public MeterTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}