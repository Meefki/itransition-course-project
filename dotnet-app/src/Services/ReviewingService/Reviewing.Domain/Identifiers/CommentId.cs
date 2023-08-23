using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Identifiers;

public sealed class CommentId
    : EntityIdentifier<Guid>
{
    public CommentId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}