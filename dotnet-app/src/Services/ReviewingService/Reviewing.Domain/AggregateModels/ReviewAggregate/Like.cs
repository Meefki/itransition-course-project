using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public class Like
    : Entity<Guid>
{
    private Like() : base(null!) { }

    public Like(Guid id) 
        : base(UserId.Create<UserId>(id))
    { }

    private UserId id = null!;
    public override EntityIdentifier<Guid> Id
    {
        get => id;
        init => id = (UserId)value;
    }

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();
}