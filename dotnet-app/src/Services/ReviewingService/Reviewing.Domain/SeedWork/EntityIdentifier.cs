namespace Reviewing.Domain.SeedWork;

public abstract class EntityIdentifier<T>
    : ValueObject
{
    public abstract T Value { get; }

    public override string ToString()
    {
        return Value!.ToString()!;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static TIdentifier Create<TIdentifier>(T value)
        where TIdentifier : EntityIdentifier<T>
        => (TIdentifier)Activator.CreateInstance(typeof(TIdentifier), new[] { value })!;
}
