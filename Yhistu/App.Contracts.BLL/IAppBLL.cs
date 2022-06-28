using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IApartmentPersonService ApartmentPersons { get; }
    IApartmentService Apartments { get; }
    IAssociationService Associations { get; }
    IAssociationServiceService AssociationServices { get; }
    IBankAccountService BankAccounts { get; }
    IBuildingService Buildings { get; }
    ICalculationRulesService CalculationRules { get; }
    IContactService Contacts { get; }
    IContactTypeService ContactTypes { get; }
    IInvoiceRowService InvoiceRows { get; }
    IInvoiceService Invoices { get; }
    IInvoiceTypeService InvoiceTypes { get; }
    IMeasuringUnitService MeasuringUnits { get; }
    IMemberService Members { get; }
    IMemberTypeService MemberTypes { get; }
    IMessageService Messages { get; }
    IMeterReadingService MeterReadings { get; }
    IMeterService Meters { get; }
    IMeterTypeService MeterTypes { get; }
    IPerkService Perks { get; }
    IPerkTypeService PerkTypes { get; }
    IPersonService Persons { get; }
    IRelationshipTypeService RelationshipTypes { get; }
    IServiceProviderService ServiceProviders { get; }
    IServiceService Services { get; }
    IServiceTypeService ServiceTypes { get; }
    
    
}