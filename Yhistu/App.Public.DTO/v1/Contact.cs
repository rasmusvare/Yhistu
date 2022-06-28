using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Contact : DomainEntityId
{
    public Guid? PersonId { get; set; }
    // public Person? Person { get; set; }

    public Guid? ContactTypeId { get; set; }
    public ContactType? ContactType { get; set; } = default!;

    public Guid? BuildingId { get; set; }
    // public Public.DTO.v1.Building? Building { get; set; }

    public Guid? ServiceProviderId { get; set; }
    // public ServiceProvider? ServiceProvider { get; set; }

    public Guid? AssociationId { get; set; }
    // public BLL.DTO.Association? Association { get; set; }

    [MaxLength(256)] public string Value { get; set; } = default!;
}