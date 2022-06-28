using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class MeterType : DomainEntityId
{
    public Guid MeasuringUnitId { get; set; }
    public MeasuringUnit? MeasuringUnit { get; set; }
    
    public Guid? AssociationId { get; set; }
    // public Association? Association { get; set; }

    [MaxLength(128)]
    public string Name { get; set; } = default!;
    // public char Type { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; } = default!;

    public int DaysBtwChecks { get; set; }

    // public ICollection<Meter>? Meters { get; set; }
}