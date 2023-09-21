using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Identifiers;

public sealed class SubjectId
    : EntityIdentifier<Guid>
{
    public SubjectId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}