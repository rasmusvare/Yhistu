using AutoMapper;

namespace App.Public.DTO.v1.MapperConfig;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.Public.DTO.v1.Apartment, App.BLL.DTO.Apartment>().ReverseMap();
        CreateMap<App.Public.DTO.v1.ApartmentPerson, App.BLL.DTO.ApartmentPerson>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Association, App.BLL.DTO.Association>().ReverseMap();
        CreateMap<App.Public.DTO.v1.AssociationService, App.BLL.DTO.AssociationService>().ReverseMap();
        CreateMap<App.Public.DTO.v1.BankAccount, App.BLL.DTO.BankAccount>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Building, App.BLL.DTO.Building>().ReverseMap();
        CreateMap<App.Public.DTO.v1.CalculationRules, App.BLL.DTO.CalculationRules>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Contact, App.BLL.DTO.Contact>().ReverseMap();
        CreateMap<App.Public.DTO.v1.ContactType, App.BLL.DTO.ContactType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Invoice, App.BLL.DTO.Invoice>().ReverseMap();
        CreateMap<App.Public.DTO.v1.InvoiceRow, App.BLL.DTO.InvoiceRow>().ReverseMap();
        CreateMap<App.Public.DTO.v1.InvoiceType, App.BLL.DTO.InvoiceType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.MeasuringUnit, App.BLL.DTO.MeasuringUnit>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Member, App.BLL.DTO.Member>().ReverseMap();
        CreateMap<App.Public.DTO.v1.MemberType, App.BLL.DTO.MemberType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Message, App.BLL.DTO.Message>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Meter, App.BLL.DTO.Meter>().ReverseMap();
        CreateMap<App.Public.DTO.v1.MeterReading, App.BLL.DTO.MeterReading>().ReverseMap();
        CreateMap<App.Public.DTO.v1.MeterType, App.BLL.DTO.MeterType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Perk, App.BLL.DTO.Perk>().ReverseMap();
        CreateMap<App.Public.DTO.v1.PerkType, App.BLL.DTO.PerkType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Person, App.BLL.DTO.Person>().ReverseMap();
        CreateMap<App.Public.DTO.v1.RelationshipType, App.BLL.DTO.RelationshipType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Service, App.BLL.DTO.Service>().ReverseMap();
        CreateMap<App.Public.DTO.v1.ServiceProvider, App.BLL.DTO.ServiceProvider>().ReverseMap();
        CreateMap<App.Public.DTO.v1.ServiceType, App.BLL.DTO.ServiceType>().ReverseMap();
    }
    
}