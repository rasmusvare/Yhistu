using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ContactTypeMapper : BaseMapper<App.BLL.DTO.ContactType, App.DAL.DTO.ContactType>
{
    public ContactTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}