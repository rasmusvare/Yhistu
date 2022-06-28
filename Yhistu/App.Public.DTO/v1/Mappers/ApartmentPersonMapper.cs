using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ApartmentPersonMapper : BaseMapper<ApartmentPerson, App.BLL.DTO.ApartmentPerson>
{
    public ApartmentPersonMapper(IMapper mapper) : base(mapper)
    {
    }
}