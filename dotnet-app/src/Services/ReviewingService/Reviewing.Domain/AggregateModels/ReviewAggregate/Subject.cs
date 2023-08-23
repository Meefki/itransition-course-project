using Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public sealed class Subject
    : ValueObject
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Subject() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Subject(
        string name,
        SubjectGroups group,
        int grade)
    {
        Name = name;
        Group = group;
        Grade = grade;
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

    //public Subject ChangeName(string name)
    //    => new(name, Group, Grade);

    //public Subject ChangeGroup(SubjectGroups group)
    //    => new(Name, group, Grade);

    //public Subject ChangeGrage(int grade)
    //    => new(Name, Group, grade);

    public static Subject Create(string name, SubjectGroups group, int grade)
        => new(name, group, grade);

    #region Value Object
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Group;
        yield return Grade;
    }

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();

    public static bool operator ==(Subject left, Subject right)
        => EqualOperator(left, right);
    public static bool operator !=(Subject left, Subject right)
        => NotEqualOperator(left, right);
    #endregion
}