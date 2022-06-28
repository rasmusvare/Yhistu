using Base.Domain;

namespace App.BLL.DTO;

public class ServiceProvider : DomainEntityId
{
    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }

    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string ContractNr { get; set; } = default!;

    public DateOnly ContractStart { get; set; }
    public DateOnly? ContractEnd { get; set; }

    public ICollection<AssociationService>? AssociationServices { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
    public ICollection<Contact>? Contacts { get; set; }
}