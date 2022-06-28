using Base.Domain;

namespace App.Public.DTO.v1;

public class Member: DomainEntityId
{
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }

    public Guid AssociationId { get; set; }
    // public BLL.DTO.Association? Association { get; set; }

    public Guid MemberTypeId { get; set; }
    public MemberType? MemberType { get; set; }
  
    public bool ViewAsRegularUser { get; set; }

    public DateOnly From { get; set; }
    public DateOnly? To { get; set; }

    public string? email { get; set; }
}