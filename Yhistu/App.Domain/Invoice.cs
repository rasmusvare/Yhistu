using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Invoice: DomainEntityMetaId
{
    public Guid AssociationId { get; set; }
    public Association? Association { get; set; }

    public Guid? ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }

    public Guid? PersonId { get; set; }
    public Person? Person { get; set; }

    public Guid? ServiceProviderId { get; set; }
    public ServiceProvider? ServiceProvider { get; set; }

    public Guid InvoiceTypeId { get; set; }
    public InvoiceType? InvoiceType { get; set; }

    [MaxLength(128)]
    public string InvoiceNumber { get; set; } = default!;
    public DateOnly InvoiceDate { get; set; }
    public DateOnly DueDate { get; set; }
    public decimal SumWithoutVat { get; set; }
    public decimal SumWithVat { get; set; }

    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}