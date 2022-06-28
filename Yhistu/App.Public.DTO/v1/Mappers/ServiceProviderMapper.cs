using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ServiceProviderMapper : BaseMapper<ServiceProvider, App.BLL.DTO.ServiceProvider>
{
    public ServiceProviderMapper(IMapper mapper) : base(mapper)
    {
    }
}