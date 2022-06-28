using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Service : DomainEntityMetaId
{
    public Guid ServiceTypeId { get; set; }
    public ServiceType? ServiceType { get; set; }

    public Guid MeasuringUnitId { get; set; }
    public MeasuringUnit? MeasuringUnit { get; set; }

    public Guid CalculationRulesId { get; set; }
    public CalculationRules? CalculationRules { get; set; }

    [MaxLength(128)] public string Name { get; set; } = default!;
    [MaxLength(1024)] public string Description { get; set; } = default!;

    public ICollection<ServiceProvider>? ServiceProviders { get; set; }
    public ICollection<AssociationService>? AssociationServices { get; set; }
    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}