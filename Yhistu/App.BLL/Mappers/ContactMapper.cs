using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ContactMapper : BaseMapper<App.BLL.DTO.Contact, App.DAL.DTO.Contact>
{
    public ContactMapper(IMapper mapper) : base(mapper)
    {
    }
}