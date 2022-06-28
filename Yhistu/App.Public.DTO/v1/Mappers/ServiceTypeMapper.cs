using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ServiceTypeMapper : BaseMapper<ServiceType, App.BLL.DTO.ServiceType>
{
    public ServiceTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}