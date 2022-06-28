using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class BankAccount : DomainEntityId
{
    public Guid AssociationId { get; set; }
    // public Association? Association { get; set; }

    [MaxLength(128)] public string Bank { get; set; } = default!;
    [MaxLength(128)] public string AccountNo { get; set; } = default!;
}