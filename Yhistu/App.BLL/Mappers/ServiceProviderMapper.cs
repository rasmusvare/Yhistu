using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ServiceProviderMapper : BaseMapper<App.BLL.DTO.ServiceProvider, App.DAL.DTO.ServiceProvider>
{
    public ServiceProviderMapper(IMapper mapper) : base(mapper)
    {
    }
}