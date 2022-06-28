using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class InvoiceMapper : BaseMapper<Invoice, App.BLL.DTO.Invoice>
{
    public InvoiceMapper(IMapper mapper) : base(mapper)
    {
    }
}