using Base.Domain;

namespace App.DAL.DTO;

public class MeterReading : DomainEntityId
{
    public Guid MeterId { get; set; }
    public Meter? Meter { get; set; }

    public decimal Value { get; set; }
    public DateOnly Date { get; set; }

    public bool AutoGenerated { get; set; }

    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}