using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Enumerations;

public sealed class SubjectGroups
    : Enumeration
{
    public SubjectGroups(int id, string name) : base(id, name) { }


    public static bool operator ==(SubjectGroups? right, SubjectGroups? left) => right.Equals(left);
    public static bool operator !=(SubjectGroups? right, SubjectGroups? left) => !right.Equals(left);

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}