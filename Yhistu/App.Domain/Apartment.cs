using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Apartment : DomainEntityMetaId
{
    public string AptNumber { get; set; } = default!;
    public decimal TotalSqMtr { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Apartment), Name = nameof(NoOfRooms))]
    public int NoOfRooms { get; set; }
    public bool IsBusiness { get; set; }

    // public char Type { get; set; } // Kas vaja?

    public Guid BuildingId { get; set; }
    public Building? Building { get; set; }

    public ICollection<Meter>? Meters { get; set; }
    public ICollection<ApartmentPerson>? ApartmentPersons { get; set; }
    public ICollection<Perk>? Perks { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
}