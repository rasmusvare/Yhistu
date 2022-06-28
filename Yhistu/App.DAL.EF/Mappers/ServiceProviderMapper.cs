using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ServiceProviderMapper : BaseMapper<App.DAL.DTO.ServiceProvider, App.Domain.ServiceProvider>
{
    public ServiceProviderMapper(IMapper mapper) : base(mapper)
    {
    }
}