using Base.Domain;
using Base.Extensions;

namespace App.DAL.DTO;

public class Building: DomainEntityId
{
    public Guid AssociationId { get; set; }
    [SkipProperty]
    public Association? Association { get; set; }
    public string Name { get; set; } = default!;

    public int? NoOfApartments { get; set; }
    public decimal CommonSqM { get; set; }
    public decimal? ApartmentSqM { get; set; }
    public decimal? BusinessSqM { get; set; }
    public decimal? TotalSqM { get; set; }

    [SkipProperty]
    public ICollection<Apartment>? Apartments { get; set; }
    [SkipProperty]
    public ICollection<Meter>? Meters { get; set; }
    [SkipProperty]
    public ICollection<Contact>? Contacts { get; set; }

}
