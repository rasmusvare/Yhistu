using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Meter : DomainEntityId
{
    public Guid? ApartmentId { get; set; }
    // public Public.DTO.v1.Apartment? Apartment { get; set; }

    public Guid BuildingId { get; set; }
    // public Public.DTO.v1.Building? Building { get; set; }

    public Guid MeterTypeId { get; set; }
    public MeterType? MeterType { get; set; }

    [MaxLength(64)]
    public string MeterNumber { get; set; } = default!;

    public DateOnly InstalledOn { get; set; }
    public DateOnly? CheckedOn { get; set; }
    public DateOnly? NextCheck { get; set; }

    public ICollection<MeterReading>? MeterReadings { get; set; }
    // public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}