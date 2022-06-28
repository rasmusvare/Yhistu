using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class PersonMapper : BaseMapper<App.DAL.DTO.Person, App.Domain.Person>
{
    public PersonMapper(IMapper mapper) : base(mapper)
    {
    }
}