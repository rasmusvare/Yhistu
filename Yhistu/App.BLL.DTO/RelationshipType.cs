using Base.Domain;

namespace App.BLL.DTO;

public class RelationshipType : DomainEntityId
{
    public string Name { get; set; } = default!;
    public bool IsOwner { get; set; }

    public ICollection<ApartmentPerson>? ApartmentPersons { get; set; }
}