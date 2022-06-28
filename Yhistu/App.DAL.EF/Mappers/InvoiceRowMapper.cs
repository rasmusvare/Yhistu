using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class InvoiceRowMapper : BaseMapper<App.DAL.DTO.InvoiceRow, App.Domain.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}