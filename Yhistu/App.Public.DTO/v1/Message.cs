using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Message : DomainEntityId
{
    public Guid AssociationId { get; set; }
    // public BLL.DTO.Association? Association { get; set; }

    public Guid PersonId { get; set; }
    // public Person? Person { get; set; }

    public Guid? MessageId { get; set; }
    // public Message? ParentMessage { get; set; }

    [MaxLength(4096)]
    public string Value { get; set; } = default!;
    
    public bool Announcement { get; set; }
    public bool ContactBoard { get; set; }
    public bool RegularMessage { get; set; }

    public ICollection<Message>? ChildMessages { get; set; }
}