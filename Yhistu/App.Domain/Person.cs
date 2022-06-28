using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Person : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? User { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    [StringLength(maximumLength: 32, MinimumLength = 11, ErrorMessage = "Wrong length on ID code")]
    public string IdCode { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsMain { get; set; }
    // public bool IsOwner { get; set; }

    public ICollection<ApartmentPerson>? ApartmentPersons { get; set; }
    public ICollection<Contact>? Contacts { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
    public ICollection<Perk>? Perks { get; set; }
    public ICollection<Message>? Messages { get; set; }
    public ICollection<Member>? Members { get; set; }
}