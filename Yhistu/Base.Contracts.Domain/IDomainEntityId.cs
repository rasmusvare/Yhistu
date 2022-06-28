namespace Base.Contracts.Domain;

/// <summary>
/// Defaut GUID based doamin interface
/// </summary>
public interface IDomainEntityId : IDomainEntityId<Guid>
{
    
}

/// <summary>
/// Universal Domain Entity interface based on generic PK type
/// </summary>
/// <typeparam name="TKey">Type for PL</typeparam>
public interface IDomainEntityId<TKey>
where TKey: IEquatable<TKey>
{
    public TKey Id { get; set; }
}