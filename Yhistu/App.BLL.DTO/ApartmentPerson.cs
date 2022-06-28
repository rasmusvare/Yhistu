using Base.Domain;

namespace App.BLL.DTO;

public class ApartmentPerson : DomainEntityId
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