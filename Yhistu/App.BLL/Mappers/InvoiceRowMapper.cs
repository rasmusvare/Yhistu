using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class InvoiceRowMapper : BaseMapper<App.BLL.DTO.InvoiceRow, App.DAL.DTO.InvoiceRow>
{
    public InvoiceRowMapper(IMapper mapper) : base(mapper)
    {
    }
}