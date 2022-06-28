using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;
    
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override async Task<int> SaveChangesAsync()
    {
        return await _unitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return _unitOfWork.SaveChanges();
    }
    
    private IApartmentPersonService? _apartmentPersons;
    public IApartmentPersonService ApartmentPersons =>
        _apartmentPersons ??= new ApartmentPersonService(_unitOfWork.ApartmentPersons, new ApartmentPersonMapper(_mapper));
    
    
    private IApartmentService? _apartments;
    public IApartmentService Apartments =>
        _apartments ??= new ApartmentService(_unitOfWork.Apartments, new ApartmentMapper(_mapper));

    
    private IAssociationService? _associations;
    public IAssociationService Associations =>
        _associations ??= new AssociationService(_unitOfWork.Associations, new AssociationMapper(_mapper));
    
    
    private IAssociationServiceService? _associationServices;
    public IAssociationServiceService AssociationServices =>
        _associationServices ??= new AssociationServiceService(_unitOfWork.AssociationServices, new AssociationServiceMapper(_mapper));

    
    private IBankAccountService? _bankAccounts;
    public IBankAccountService BankAccounts =>
        _bankAccounts ??= new BankAccountService(_unitOfWork.BankAccounts, new BankAccountMapper(_mapper));
    
    
    private IBuildingService? _buildings;
    public IBuildingService Buildings =>
        _buildings ??= new BuildingService(_unitOfWork.Buildings, new BuildingMapper(_mapper));
    
    
    private ICalculationRulesService? _calculationRules;
    public ICalculationRulesService CalculationRules =>
        _calculationRules ??= new CalculationRulesService(_unitOfWork.CalculationRules, new CalculationRulesMapper(_mapper));
    
    
    private IContactService? _contacts;
    public IContactService Contacts =>
        _contacts ??= new ContactService(_unitOfWork.Contacts, new ContactMapper(_mapper));
    
    
    private IContactTypeService? _contactTypes;
    public IContactTypeService ContactTypes =>
        _contactTypes ??= new ContactTypeService(_unitOfWork.ContactTypes, new ContactTypeMapper(_mapper));
    
    
    private IInvoiceRowService? _invoiceRows;
    public IInvoiceRowService InvoiceRows =>
        _invoiceRows ??= new InvoiceRowService(_unitOfWork.InvoiceRows, new InvoiceRowMapper(_mapper));
    
    
    private IInvoiceService? _invoices;
    public IInvoiceService Invoices =>
        _invoices ??= new InvoiceService(_unitOfWork.Invoices, new InvoiceMapper(_mapper));
    
    
    private IInvoiceTypeService? _invoiceTypes;
    public IInvoiceTypeService InvoiceTypes =>
        _invoiceTypes ??= new InvoiceTypeService(_unitOfWork.InvoiceTypes, new InvoiceTypeMapper(_mapper));
    
    
    private IMeasuringUnitService? _measuringUnits;
    public IMeasuringUnitService MeasuringUnits =>
        _measuringUnits ??= new MeasuringUnitService(_unitOfWork.MeasuringUnits, new MeasuringUnitMapper(_mapper));
    

    private IMemberService? _members;
    public IMemberService Members =>
        _members ??= new MemberService(_unitOfWork.Members, new MemberMapper(_mapper));

    
    private IMemberTypeService? _memberTypes;
    public IMemberTypeService MemberTypes =>
        _memberTypes ??= new MemberTypeService(_unitOfWork.MemberTypes, new MemberTypeMapper(_mapper));
    
    
    private IMessageService? _messages;
    public IMessageService Messages =>
        _messages ??= new MessageService(_unitOfWork.Messages, new MessageMapper(_mapper));


    private IMeterReadingService? _meterReadings;
    public IMeterReadingService MeterReadings =>
        _meterReadings ??= new MeterReadingService(_unitOfWork.MeterReadings, new MeterReadingMapper(_mapper));
    
    
    private IMeterService? _meters;
    public IMeterService Meters =>
        _meters ??= new MeterService(_unitOfWork.Meters, new MeterMapper(_mapper));
    
    
    private IMeterTypeService? _meterTypes;
    public IMeterTypeService MeterTypes =>
        _meterTypes ??= new MeterTypeService(_unitOfWork.MeterTypes, new MeterTypeMapper(_mapper));


    private IPerkService? _perks;
    public IPerkService Perks =>
        _perks ??= new PerkService(_unitOfWork.Perks, new PerkMapper(_mapper));
     
    
    private IPerkTypeService? _perkTypes;
    public IPerkTypeService PerkTypes =>
        _perkTypes ??= new PerkTypeService(_unitOfWork.PerkTypes, new PerkTypeMapper(_mapper));
     
    
    private IPersonService? _persons;
    public IPersonService Persons =>
        _persons ??= new PersonService(_unitOfWork.Persons, new PersonMapper(_mapper));
    

    private IRelationshipTypeService? _relationshipTypes;
    public IRelationshipTypeService RelationshipTypes =>
        _relationshipTypes ??= new RelationshipTypeService(_unitOfWork.RelationshipTypes, new RelationShipTypeMapper(_mapper));
     
    
    private IServiceProviderService? _serviceProviders;
    public IServiceProviderService ServiceProviders =>
        _serviceProviders ??= new ServiceProviderService(_unitOfWork.ServiceProviders, new ServiceProviderMapper(_mapper));


    private IServiceService? _services;
    public IServiceService Services =>
        _services ??= new ServiceService(_unitOfWork.Services, new ServiceMapper(_mapper));
     
    
    private IServiceTypeService? _serviceTypes;
    public IServiceTypeService ServiceTypes =>
        _serviceTypes ??= new ServiceTypeService(_unitOfWork.ServiceTypes, new ServiceTypeMapper(_mapper));
    
        
        
        
        
    
    
    
    

    
    
}