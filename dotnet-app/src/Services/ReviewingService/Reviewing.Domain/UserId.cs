using Comments.Domain.SeedWork;

namespace Comments.Domain;

public class UserId
    : EntityIdentifier<Guid>
{
    public UserId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}