using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Base.Domain;

namespace App.Domain;

public class ContactType : DomainEntityMetaId
{
    public Guid? AssociationId { get; set; }
    public Association? Association { get; set; }
    [MaxLength(128)] public string Name { get; set; } = default!;
    [MaxLength(128)] public string Description { get; set; } = default!;

    public ICollection<Contact>? Contacts { get; set; }
}