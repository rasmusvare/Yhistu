using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class InvoiceRowMapper : BaseMapper<InvoiceRow, App.BLL.DTO.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}