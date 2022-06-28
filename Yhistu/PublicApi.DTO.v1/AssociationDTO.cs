using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO.Domain;

public class AssociationDTO

{
    public Guid Id { get; set; }
    [MaxLength(128)] public string Name { get; set; } = default!;

    [MaxLength(128)] public string RegistrationNo { get; set; } = default!;

    public DateOnly FoundedOn { get; set; }
}