using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ContactMapper : BaseMapper<Contact, App.BLL.DTO.Contact>
{
    public ContactMapper(IMapper mapper) : base(mapper)
    {
    }
}