namespace Reviewing.API.Application.Queries.ViewModels;

public class TagVM
{
    public Guid reviewId { get; set; }
    public string name { get; set; } = null!;
}