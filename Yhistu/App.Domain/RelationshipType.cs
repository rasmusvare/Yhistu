using Base.Domain;

namespace App.Domain;

public class RelationshipType : DomainEntityMetaId
{
    public Guid AssociationId { get; set; }
    public Association? Association { get; set; }
    
    public string Name { get; set; } = default!;
    public bool IsOwner { get; set; }

    public ICollection<ApartmentPerson>? ApartmentPersons { get; set; }
}