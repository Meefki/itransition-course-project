using Reviewing.Domain.Identifiers;

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
    {
        Type type = typeof(TIdentifier);
        var instance = (Activator.CreateInstance(type, value) as TIdentifier)!;
        return instance;
    }

    #region Value Object
    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();

    public static bool operator ==(EntityIdentifier<T> left, EntityIdentifier<T> right)
        => EqualOperator(left, right);

    public static bool operator !=(EntityIdentifier<T> left, EntityIdentifier<T> right)
        => NotEqualOperator(left, right);
    #endregion
}
