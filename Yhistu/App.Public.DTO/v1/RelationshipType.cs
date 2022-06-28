using Base.Domain;

namespace App.Public.DTO.v1;

public class RelationshipType : DomainEntityId
{
    public string Name { get; set; } = default!;
    public bool IsOwner { get; set; }

    public ICollection<ApartmentPerson>? ApartmentPersons { get; set; }
}