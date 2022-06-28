using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class InvoiceTypeRepository :
    BaseEntityRepository<App.DAL.DTO.InvoiceType, App.Domain.InvoiceType, AppDbContext>, IInvoiceTypeRepository
{
    public InvoiceTypeRepository(AppDbContext dbContext, IMapper<InvoiceType, Domain.InvoiceType> mapper) : base(
        dbContext, mapper)
    {
    }
}