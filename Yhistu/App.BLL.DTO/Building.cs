using Base.Domain;

namespace App.BLL.DTO;

public class Building: DomainEntityId
{
    public Guid AssociationId { get; set; }
    // public Association? Association { get; set; }
    public string Name { get; set; } = default!;

    public int? NoOfApartments { get; set; }
    public decimal CommonSqM { get; set; } 
    public decimal? ApartmentSqM { get; set; }
    public decimal? BusinessSqM { get; set; }
    public decimal? TotalSqM { get; set; }

    // public ICollection<Apartment>? Apartments { get; set; }
    // public ICollection<Meter>? Meters { get; set; }
    // public ICollection<Contact>? Contacts { get; set; }

}