using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ServiceTypeRepository :
    BaseEntityRepository<App.DAL.DTO.ServiceType, App.Domain.ServiceType, AppDbContext>, IServiceTypeRepository
{
    public ServiceTypeRepository(AppDbContext dbContext, IMapper<ServiceType, Domain.ServiceType> mapper) : base(
        dbContext, mapper)
    {
    }
}