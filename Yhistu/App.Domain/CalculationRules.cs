using Base.Domain;

namespace App.Domain;

public class CalculationRules: DomainEntityMetaId
{
    public string Name { get; set; } = default!;

    public bool FixedPrice { get; set; }
    public bool CalculatePerAptSqM { get; set; }
    public bool CalculatePerApt { get; set; }
    public bool CalculatePerReading { get; set; }
    public bool CalculatePerCommonSqM { get; set; }
    
}