using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class InvoiceRepository : BaseEntityRepository<App.DAL.DTO.Invoice, App.Domain.Invoice, AppDbContext>,
    IInvoiceRepository
{
    public InvoiceRepository(AppDbContext dbContext, IMapper<Invoice, Domain.Invoice> mapper) : base(dbContext, mapper)
    {
    }
}