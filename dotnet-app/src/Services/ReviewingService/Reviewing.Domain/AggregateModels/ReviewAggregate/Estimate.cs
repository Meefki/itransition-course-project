using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public class Estimate
    : Entity<Guid>
{
    private Estimate() : base(null!) { }

    public Estimate(
        UserId userId,
        int grade)
        : base(userId) => Grade = grade;

    public override EntityIdentifier<Guid> Id { get; init; } = null!;
    public int Grade { get; private set; }

    public void ChangeGrade(int grade)
    {
        if (grade <= 5 && grade > 0)
            Grade = grade;
    }
}