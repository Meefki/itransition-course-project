using Comments.Domain.SeedWork;

namespace Comments.Domain;

public class ReviewId 
    : EntityIdentifier<Guid>
{
    public ReviewId(Guid Value)
    {
        this.Value = Value;
    }

    public override Guid Value { get; }
}