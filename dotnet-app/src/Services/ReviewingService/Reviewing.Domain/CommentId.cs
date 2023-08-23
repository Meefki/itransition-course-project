using Comments.Domain.SeedWork;

namespace Comments.Domain;

public class CommentId
    : EntityIdentifier<Guid>
{
    public CommentId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}