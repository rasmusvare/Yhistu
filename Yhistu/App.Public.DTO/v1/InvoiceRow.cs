using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class InvoiceRow: DomainEntityId
{
    public Guid InvoiceId { get; set; }
    // public Invoice? Invoice { get; set; }

    public Guid? MeterReadingId { get; set; }
    // public MeterReading? MeterReading { get; set; }

    public Guid? MeterId { get; set; }
    // public Meter? Meter { get; set; }

    public Guid? ServiceId { get; set; }
    // public Service? Service { get; set; }

    [MaxLength(128)] public string Name { get; set; } = default!;
    public decimal Sum { get; set; }
}