using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<BLL.DTO.Apartment, DAL.DTO.Apartment>().ReverseMap();
        CreateMap<BLL.DTO.ApartmentPerson, DAL.DTO.ApartmentPerson>().ReverseMap();
        CreateMap<BLL.DTO.Association, DAL.DTO.Association>().ReverseMap();
        CreateMap<BLL.DTO.AssociationService, DAL.DTO.AssociationService>().ReverseMap();
        CreateMap<BLL.DTO.BankAccount, DAL.DTO.BankAccount>().ReverseMap();
        CreateMap<BLL.DTO.Building, DAL.DTO.Building>().ReverseMap();
        CreateMap<BLL.DTO.CalculationRules, DAL.DTO.CalculationRules>().ReverseMap();
        CreateMap<BLL.DTO.Contact, DAL.DTO.Contact>().ReverseMap();
        CreateMap<BLL.DTO.ContactType, DAL.DTO.ContactType>().ReverseMap();
        CreateMap<BLL.DTO.Invoice, DAL.DTO.Invoice>().ReverseMap();
        CreateMap<BLL.DTO.InvoiceRow, DAL.DTO.InvoiceRow>().ReverseMap();
        CreateMap<BLL.DTO.InvoiceType, DAL.DTO.InvoiceType>().ReverseMap();
        CreateMap<BLL.DTO.MeasuringUnit, DAL.DTO.MeasuringUnit>().ReverseMap();
        CreateMap<BLL.DTO.Member, DAL.DTO.Member>().ReverseMap();
        CreateMap<BLL.DTO.MemberType, DAL.DTO.MemberType>().ReverseMap();
        CreateMap<BLL.DTO.Message, DAL.DTO.Message>().ReverseMap();
        CreateMap<BLL.DTO.Meter, DAL.DTO.Meter>().ReverseMap();
        CreateMap<BLL.DTO.MeterReading, DAL.DTO.MeterReading>().ReverseMap();
        CreateMap<BLL.DTO.MeterType, DAL.DTO.MeterType>().ReverseMap();
        CreateMap<BLL.DTO.Perk, DAL.DTO.Perk>().ReverseMap();
        CreateMap<BLL.DTO.PerkType, DAL.DTO.PerkType>().ReverseMap();
        CreateMap<BLL.DTO.Person, DAL.DTO.Person>().ReverseMap();
        CreateMap<BLL.DTO.RelationshipType, DAL.DTO.RelationshipType>().ReverseMap();
        CreateMap<BLL.DTO.Service, DAL.DTO.Service>().ReverseMap();
        CreateMap<BLL.DTO.ServiceProvider, DAL.DTO.ServiceProvider>().ReverseMap();
        CreateMap<BLL.DTO.ServiceType, DAL.DTO.ServiceType>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
    }
    
}