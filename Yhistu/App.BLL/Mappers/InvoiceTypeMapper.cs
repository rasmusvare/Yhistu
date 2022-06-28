using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class InvoiceTypeMapper : BaseMapper<App.BLL.DTO.InvoiceType, App.DAL.DTO.InvoiceType>
{
    public InvoiceTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}