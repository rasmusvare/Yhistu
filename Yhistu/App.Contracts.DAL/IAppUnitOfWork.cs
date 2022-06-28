using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
     IApartmentRepository Apartments { get; }
     IApartmentPersonRepository ApartmentPersons { get; }
     IAssociationRepository Associations { get; }
     IAssociationServiceRepository AssociationServices { get; }
     IBankAccountRepository BankAccounts { get; }
     IBuildingRepository Buildings { get; }
     ICalculationRulesRepository CalculationRules { get; }
     IContactRepository Contacts { get; }
     IContactTypeRepository ContactTypes { get; }
     IInvoiceRepository Invoices { get; }
     IInvoiceRowRepository InvoiceRows { get; }
     IInvoiceTypeRepository InvoiceTypes { get; }
     IMeasuringUnitRepository MeasuringUnits { get; }
     IMemberRepository Members { get; }
     IMemberTypeRepository MemberTypes { get; }
     IMessageRepository Messages { get; }
     IMeterRepository Meters { get; }
     IMeterReadingRepository MeterReadings { get; }
     IMeterTypeRepository MeterTypes { get; }
     IPerkRepository Perks { get; }
     IPerkTypeRepository PerkTypes { get; }
     IPersonRepository Persons { get; }
     IRelationshipTypeRepository RelationshipTypes { get; }
     IServiceRepository Services { get; }
     IServiceProviderRepository ServiceProviders { get; }
     IServiceTypeRepository ServiceTypes { get; }
}