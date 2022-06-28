using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class ApartmentMapper : BaseMapper<Apartment, App.BLL.DTO.Apartment>
{
    public ApartmentMapper(IMapper mapper) : base(mapper)
    {
    }
}