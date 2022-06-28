using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class InvoiceType : DomainEntityId
{
    [MaxLength(128)] public string Name { get; set; } = default!;
    public char Type { get; set; }
    public int Vat { get; set; }
}