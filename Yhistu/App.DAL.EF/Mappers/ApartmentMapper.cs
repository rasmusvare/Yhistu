using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class ApartmentMapper : BaseMapper<App.DAL.DTO.Apartment, App.Domain.Apartment>
{
    public ApartmentMapper(IMapper mapper) : base(mapper)
    {
    }
}