using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class InvoiceRowService :
    BaseEntityService<App.BLL.DTO.InvoiceRow, App.DAL.DTO.InvoiceRow, IInvoiceRowRepository>,
    IInvoiceRowService
{
    public InvoiceRowService(IInvoiceRowRepository repository, IMapper<InvoiceRow, DAL.DTO.InvoiceRow> mapper) : base(
        repository, mapper)
    {
    }
}