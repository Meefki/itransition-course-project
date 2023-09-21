using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Identifiers;

public sealed class UserId
    : EntityIdentifier<Guid>
{
    public UserId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}