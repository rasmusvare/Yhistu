using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class Association: DomainEntityId
{
    [MaxLength(128)] public string Name { get; set; } = default!;

    [MaxLength(128)] public string RegistrationNo { get; set; } = default!;

    public DateOnly FoundedOn { get; set; }

    public ICollection<AssociationService>? AssociationServices { get; set; }
    public ICollection<BankAccount>? BankAccounts { get; set; }
    public ICollection<Member>? Members { get; set; }
    public ICollection<Building>? Buildings { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
    public ICollection<Contact>? Contacts { get; set; }
    public ICollection<Message>? Messages { get; set; }
    public ICollection<MemberType>? MemberTypes { get; set; }

}