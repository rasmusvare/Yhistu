using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class InvoiceType : DomainEntityMetaId
{
    [MaxLength(128)] public string Name { get; set; } = default!;
    public char Type { get; set; }
    public int Vat { get; set; }
}