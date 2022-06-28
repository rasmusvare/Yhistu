using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class PerkType : DomainEntityId
{
    [MaxLength(128)] public string Name { get; set; } = default!;

    [MaxLength(1024)] public string Description { get; set; } = default!;

    public ICollection<Perk>? Perks { get; set; }
}