using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ServiceRepository : BaseEntityRepository<App.DAL.DTO.Service, App.Domain.Service, AppDbContext>,
    IServiceRepository
{
    public ServiceRepository(AppDbContext dbContext, IMapper<Service, Domain.Service> mapper) : base(dbContext, mapper)
    {
    }
}