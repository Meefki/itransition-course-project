using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Enumerations;

public class ReviewStatuses
    : Enumeration
{
    public static ReviewStatuses Draft => new(1, nameof(Draft));
    public static ReviewStatuses Published => new(2, nameof(Published));
    public static ReviewStatuses Archived => new(3, nameof(Archived));

    private ReviewStatuses(int id, string name) : base(id, name) { }
}