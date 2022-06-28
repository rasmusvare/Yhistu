using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ServiceProviderService : BaseEntityService<App.BLL.DTO.ServiceProvider, App.DAL.DTO.ServiceProvider,
        IServiceProviderRepository>,
    IServiceProviderService
{
    public ServiceProviderService(IServiceProviderRepository repository,
        IMapper<ServiceProvider, DAL.DTO.ServiceProvider> mapper) : base(repository, mapper)
    {
    }
}