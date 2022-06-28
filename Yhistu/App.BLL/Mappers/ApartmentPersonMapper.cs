using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ApartmentPersonMapper : BaseMapper<App.BLL.DTO.ApartmentPerson, App.DAL.DTO.ApartmentPerson>
{
    public ApartmentPersonMapper(IMapper mapper) : base(mapper)
    {
    }
}