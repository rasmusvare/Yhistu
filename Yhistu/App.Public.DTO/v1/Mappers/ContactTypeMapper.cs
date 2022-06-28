using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ContactTypeMapper : BaseMapper<ContactType, App.BLL.DTO.ContactType>
{
    public ContactTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}