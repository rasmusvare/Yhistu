using Base.Domain;

namespace App.DAL.DTO;

public class ServiceType : DomainEntityId
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<Service>? Services { get; set; }

}