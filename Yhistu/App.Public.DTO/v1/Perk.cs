using Base.Domain;

namespace App.Public.DTO.v1;

public class Perk : DomainEntityId
{
    public Guid? PersonId { get; set; }
    // public Person? Person { get; set; }

    public Guid? ApartmentId { get; set; }
    // public Apartment? Apartment { get; set; }

    public Guid PerkTypeId { get; set; }
    // public PerkType? PerkType { get; set; }

    public Guid? BuildingId { get; set; }
    // public Public.DTO.v1.Building? Building { get; set; }

    public string Value { get; set; } = default!;
    public DateOnly From { get; set; }
    public DateOnly? To { get; set; }
}