using Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public sealed class Subject
    : Entity<Guid>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Subject() : base(null!) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Subject(
        SubjectId id,
        string name,
        SubjectGroups group,
        int grade)
        : base(id)
    {
        Name = name;
        Group = group;
        Grade = grade;
    }

    private SubjectId id = null!;
    public override EntityIdentifier<Guid> Id
    {
        get => id;
        init => id = (SubjectId)value;
    }

    public string Name { get; init; }
    public SubjectGroups Group { get; init; }

    private int grade;
    public int Grade
    {
        get => grade;
        init
        {
            if (value < 1 || value > 10)
                WrongSubjectGradeDomainException.Throw();

            grade = value;
        }
    }

    public static Subject Create(SubjectId id, string name, SubjectGroups group, int grade)
        => new(id, name, group, grade);
}