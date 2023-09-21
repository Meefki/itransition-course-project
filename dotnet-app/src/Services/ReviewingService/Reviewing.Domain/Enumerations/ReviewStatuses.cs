using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.Enumerations;

public class ReviewStatuses
    : Enumeration
{
    public static readonly ReviewStatuses Draft = new(1, nameof(Draft));
    public static readonly ReviewStatuses Published = new(2, nameof(Published));
    public static readonly ReviewStatuses Archived = new(3, nameof(Archived));
    public static readonly ReviewStatuses Banned = new(4, nameof(Banned));
    public static readonly ReviewStatuses Deleted = new(5, nameof(Deleted));

    public ReviewStatuses(int id, string name) : base(id, name) { }
}