using Base.Domain;

namespace App.Public.DTO.v1;

public class AssociationService: DomainEntityId
{
    public Guid AssociationId { get; set; }
    public BLL.DTO.Association? Association { get; set; }

    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }

    public Guid ServiceProviderId { get; set; }
    public ServiceProvider? ServiceProvider { get; set; }
}