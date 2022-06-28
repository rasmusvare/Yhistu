using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ServiceProviderRepository :
    BaseEntityRepository<App.DAL.DTO.ServiceProvider, App.Domain.ServiceProvider, AppDbContext>,
    IServiceProviderRepository
{
    public ServiceProviderRepository(AppDbContext dbContext, IMapper<ServiceProvider, Domain.ServiceProvider> mapper) :
        base(dbContext, mapper)
    {
    }
}