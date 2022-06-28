using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class InvoiceTypeService :
    BaseEntityService<App.BLL.DTO.InvoiceType, App.DAL.DTO.InvoiceType, IInvoiceTypeRepository>,
    IInvoiceTypeService
{
    public InvoiceTypeService(IInvoiceTypeRepository repository, IMapper<InvoiceType, DAL.DTO.InvoiceType> mapper) :
        base(repository, mapper)
    {
    }
}