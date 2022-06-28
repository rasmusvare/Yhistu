using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class MemberType : DomainEntityId
{
    [MaxLength(128)] public string Name { get; set; } = default!;
    [MaxLength(1024)] public string Description { get; set; } = default!;
    public bool IsMemberOfBoard { get; set; }
    public bool IsAdministrator { get; set; }
    public bool IsRegularMember { get; set; }
    public bool IsManager { get; set; }
    public bool IsAccountant { get; set; }
    
    public ICollection<Member>? Members { get; set; }
    
    public Guid AssociationId { get; set; }
    public Association? Association { get; set; }
    
}