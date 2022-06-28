using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class InvoiceTypeMapper : BaseMapper<InvoiceType, App.BLL.DTO.InvoiceType>
{
    public InvoiceTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}