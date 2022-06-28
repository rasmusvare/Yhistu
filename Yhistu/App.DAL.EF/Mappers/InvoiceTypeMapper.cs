using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class InvoiceTypeMapper : BaseMapper<App.DAL.DTO.InvoiceType, App.Domain.InvoiceType>
{
    public InvoiceTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}