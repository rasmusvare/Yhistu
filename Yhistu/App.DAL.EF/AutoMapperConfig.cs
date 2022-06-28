using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DAL.DTO.Apartment, Domain.Apartment>().ReverseMap();
        CreateMap<DAL.DTO.ApartmentPerson, Domain.ApartmentPerson>().ReverseMap();
        CreateMap<DAL.DTO.Association, Domain.Association>().ReverseMap();
        CreateMap<DAL.DTO.AssociationService, Domain.AssociationService>().ReverseMap();
        CreateMap<DAL.DTO.BankAccount, Domain.BankAccount>().ReverseMap();
        CreateMap<DAL.DTO.Building, Domain.Building>().ReverseMap();
        CreateMap<DAL.DTO.CalculationRules, Domain.CalculationRules>().ReverseMap();
        CreateMap<DAL.DTO.Contact, Domain.Contact>().ReverseMap();
        CreateMap<DAL.DTO.ContactType, Domain.ContactType>().ReverseMap();
        CreateMap<DAL.DTO.Invoice, Domain.Invoice>().ReverseMap();
        CreateMap<DAL.DTO.InvoiceRow, Domain.InvoiceRow>().ReverseMap();
        CreateMap<DAL.DTO.InvoiceType, Domain.InvoiceType>().ReverseMap();
        CreateMap<DAL.DTO.MeasuringUnit, Domain.MeasuringUnit>().ReverseMap();
        CreateMap<DAL.DTO.Member, Domain.Member>().ReverseMap();
        CreateMap<DAL.DTO.MemberType, Domain.MemberType>().ReverseMap();
        CreateMap<DAL.DTO.Message, Domain.Message>().ReverseMap();
        CreateMap<DAL.DTO.Meter, Domain.Meter>().ReverseMap();
        CreateMap<DAL.DTO.MeterReading, Domain.MeterReading>().ReverseMap();
        CreateMap<DAL.DTO.MeterType, Domain.MeterType>().ReverseMap();
        CreateMap<DAL.DTO.Perk, Domain.Perk>().ReverseMap();
        CreateMap<DAL.DTO.PerkType, Domain.PerkType>().ReverseMap();
        CreateMap<DAL.DTO.Person, Domain.Person>().ReverseMap();
        CreateMap<DAL.DTO.RelationshipType, Domain.RelationshipType>().ReverseMap();
        CreateMap<DAL.DTO.Service, Domain.Service>().ReverseMap();
        CreateMap<DAL.DTO.ServiceProvider, Domain.ServiceProvider>().ReverseMap();
        CreateMap<DAL.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
    
}