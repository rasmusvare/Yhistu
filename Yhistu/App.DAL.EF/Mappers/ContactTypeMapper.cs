using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ContactTypeMapper : BaseMapper<App.DAL.DTO.ContactType, App.Domain.ContactType>
{
    public ContactTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}