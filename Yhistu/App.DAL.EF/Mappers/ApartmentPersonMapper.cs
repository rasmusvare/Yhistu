using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartmentPersonMapper : BaseMapper<App.DAL.DTO.ApartmentPerson, App.Domain.ApartmentPerson>
{
    public ApartmentPersonMapper(IMapper mapper) : base(mapper)
    {
    }
}