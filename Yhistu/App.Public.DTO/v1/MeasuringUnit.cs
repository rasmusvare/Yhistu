using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class MeasuringUnit : DomainEntityId
{
    public Guid? AssociationId { get; set; }
    // public Association? Association { get; set; }
    
    [MaxLength(128)] public string Name { get; set; } = default!;
    [MaxLength(1024)] public string Description { get; set; } = default!;
    [MaxLength(16)] public string Symbol { get; set; } = default!;

    // public ICollection<MeterType>? MeterTypes { get; set; }
    // public ICollection<Service>? Services { get; set; }
}