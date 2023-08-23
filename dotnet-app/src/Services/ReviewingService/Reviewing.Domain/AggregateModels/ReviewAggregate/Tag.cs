using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public class Tag
    : ValueObject
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Tag() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Tag(
        string name)
    {
        Name = name;
    }

    public string Name { get; }

    #region Value Object
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();

    public static bool operator ==(Tag left, Tag right)
        => EqualOperator(left, right);

    public static bool operator !=(Tag left, Tag right)
        => NotEqualOperator(left, right);

    #endregion
}