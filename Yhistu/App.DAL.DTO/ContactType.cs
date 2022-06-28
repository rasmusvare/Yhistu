using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class ContactType : DomainEntityId
{
    public Guid? AssociationId { get; set; }
    public Association? Association { get; set; }
    
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    
    public ICollection<Contact>? Contacts { get; set; }

}