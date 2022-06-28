using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ContactMapper : BaseMapper<App.DAL.DTO.Contact, App.Domain.Contact>
{
    public ContactMapper(IMapper mapper) : base(mapper)
    {
    }
}