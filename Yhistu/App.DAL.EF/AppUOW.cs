using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    private readonly AutoMapper.IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
    
    private IApartmentRepository? _apartments;
    public virtual IApartmentRepository Apartments =>
        _apartments ??= new ApartmentRepository(UOWDbContext, new ApartmentMapper(_mapper));
    
    
    private IApartmentPersonRepository? _apartmentPersons;
    public virtual IApartmentPersonRepository ApartmentPersons =>
        _apartmentPersons ??= new ApartmentPersonRepository(UOWDbContext, new ApartmentPersonMapper(_mapper));

    
    private IAssociationRepository? _associations;
    public virtual IAssociationRepository Associations =>
        _associations ??= new AssociationRepository(UOWDbContext, new AssociationMapper(_mapper));

    
    private IAssociationServiceRepository? _associationServices;
    public virtual IAssociationServiceRepository AssociationServices =>
        _associationServices ??= new AssociationServiceRepository(UOWDbContext, new AssociationServiceMapper(_mapper));

    
    private IBankAccountRepository? _bankAccounts;
    public virtual IBankAccountRepository BankAccounts =>
        _bankAccounts ??= new BankAccountRepository(UOWDbContext, new BankAccountMapper(_mapper));

    
    private IBuildingRepository? _buildings;
    public virtual IBuildingRepository Buildings =>
        _buildings ??= new BuildingRepository(UOWDbContext, new BuildingMapper(_mapper));
    
    
    private ICalculationRulesRepository? _calculationRules;
    public virtual ICalculationRulesRepository CalculationRules =>
        _calculationRules ??= new CalculationRulesRepository(UOWDbContext, new CalculationRulesMapper(_mapper));
    
    
    private IContactRepository? _contacts;
    public virtual IContactRepository Contacts =>
        _contacts ??= new ContactRepository(UOWDbContext, new ContactMapper(_mapper));
    
    
    private IContactTypeRepository? _contactTypes;
    public virtual IContactTypeRepository ContactTypes =>
        _contactTypes ??= new ContactTypeRepository(UOWDbContext, new ContactTypeMapper(_mapper));
    
    
    private IInvoiceRepository? _invoices;
    public virtual IInvoiceRepository Invoices =>
        _invoices ??= new InvoiceRepository(UOWDbContext, new InvoiceMapper(_mapper));
    
    
    private IInvoiceRowRepository? _invoiceRows;
    public virtual IInvoiceRowRepository InvoiceRows =>
        _invoiceRows ??= new InvoiceRowRepository(UOWDbContext, new InvoiceRowMapper(_mapper));
    
    
    private IInvoiceTypeRepository? _invoiceTypes;
    public virtual IInvoiceTypeRepository InvoiceTypes =>
        _invoiceTypes ??= new InvoiceTypeRepository(UOWDbContext, new InvoiceTypeMapper(_mapper));
  
    
    private IMeasuringUnitRepository? _measuringUnits;
    public virtual IMeasuringUnitRepository MeasuringUnits =>
        _measuringUnits ??= new MeasuringUnitRepository(UOWDbContext, new MeasuringUnitMapper(_mapper));
    
    
    private IMemberRepository? _members;
    public virtual IMemberRepository Members =>
        _members ??= new MemberRepository(UOWDbContext, new MemberMapper(_mapper));

    
    private IMemberTypeRepository? _memberTypes;
    public virtual IMemberTypeRepository MemberTypes =>
        _memberTypes ??= new MemberTypeRepository(UOWDbContext, new MemberTypeMapper(_mapper));
    
    
    private IMessageRepository? _messages;
    public virtual IMessageRepository Messages =>
        _messages ??= new MessageRepository(UOWDbContext, new MessageMapper(_mapper));
    
    
    private IMeterRepository? _meters;
    public virtual IMeterRepository Meters =>
        _meters ??= new MeterRepository(UOWDbContext, new MeterMapper(_mapper));
    
    
    private IMeterReadingRepository? _meterReadings;
    public virtual IMeterReadingRepository MeterReadings =>
        _meterReadings ??= new MeterReadingRepository(UOWDbContext, new MeterReadingMapper(_mapper));
    
    
    private IMeterTypeRepository? _meterTypes;
    public virtual IMeterTypeRepository MeterTypes =>
        _meterTypes ??= new MeterTypeRepository(UOWDbContext, new MeterTypeMapper(_mapper));
    
    
    private IPerkRepository? _perks;
    public virtual IPerkRepository Perks =>
        _perks ??= new PerkRepository(UOWDbContext, new PerkMapper(_mapper));
    
    
    private IPerkTypeRepository? _perkTypes;
    public virtual IPerkTypeRepository PerkTypes =>
        _perkTypes ??= new PerkTypeRepository(UOWDbContext, new PerkTypeMapper(_mapper));
    
    
    private IPersonRepository? _persons;
    public virtual IPersonRepository Persons =>
        _persons ??= new PersonRepository(UOWDbContext, new PersonMapper(_mapper));
    
    
    private IRelationshipTypeRepository? _relationshipTypes;
    public virtual IRelationshipTypeRepository RelationshipTypes =>
        _relationshipTypes ??= new RelationshipTypeRepository(UOWDbContext, new RelationShipTypeMapper(_mapper));
    
    
    private IServiceRepository? _services;
    public virtual IServiceRepository Services =>
        _services ??= new ServiceRepository(UOWDbContext, new ServiceMapper(_mapper));
    
    private IServiceProviderRepository? _serviceProviders;
    public virtual IServiceProviderRepository ServiceProviders =>
        _serviceProviders ??= new ServiceProviderRepository(UOWDbContext, new ServiceProviderMapper(_mapper));
    
    
    private IServiceTypeRepository? _serviceTypes;
    public virtual IServiceTypeRepository ServiceTypes =>
        _serviceTypes ??= new ServiceTypeRepository(UOWDbContext, new ServiceTypeMapper(_mapper));
}