using Base.Domain;

namespace App.Domain;

public class ApartmentPerson : DomainEntityMetaId
{
    public Guid ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
    
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }

    public bool IsOwner { get; set; }

    // public Guid RelationshipTypeId { get; set; }
    // public RelationshipType? RelationshipType { get; set; }

    public DateOnly From { get; set; }
    public DateOnly? To { get; set; }
}