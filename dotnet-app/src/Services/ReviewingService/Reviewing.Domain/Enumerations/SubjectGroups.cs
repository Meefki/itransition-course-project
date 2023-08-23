using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Enumerations;

public sealed class SubjectGroups
    : Enumeration
{
    public SubjectGroups(int id, string name) : base(id, name) { }
}