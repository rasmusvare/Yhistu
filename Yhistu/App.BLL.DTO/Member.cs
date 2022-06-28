using Base.Domain;

namespace App.BLL.DTO;

public class Member: DomainEntityId
{
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }

    public Guid AssociationId { get; set; }
    public Association? Association { get; set; }

    public Guid MemberTypeId { get; set; }
    public MemberType? MemberType { get; set; }
    
    public bool ViewAsRegularUser { get; set; }

    public DateOnly From { get; set; }
    public DateOnly? To { get; set; }
}