using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class InvoiceRowRepository : BaseEntityRepository<App.DAL.DTO.InvoiceRow, App.Domain.InvoiceRow, AppDbContext>,
    IInvoiceRowRepository
{
    public InvoiceRowRepository(AppDbContext dbContext, IMapper<InvoiceRow, Domain.InvoiceRow> mapper) : base(dbContext,
        mapper)
    {
    }
}