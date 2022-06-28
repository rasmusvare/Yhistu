using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ServiceMapper : BaseMapper<Service, App.BLL.DTO.Service>
{
    public ServiceMapper(IMapper mapper) : base(mapper)
    {
    }
}