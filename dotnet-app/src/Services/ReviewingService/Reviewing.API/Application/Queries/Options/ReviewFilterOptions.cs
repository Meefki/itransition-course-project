namespace Reviewing.API.Application.Queries.Options;

public class ReviewFilterOptions
{
    public string? Name { get; set; } = null;
    public string? SubjectName { get; set; } = null;
    public string? Status { get; set; } = null;
    public List<string>? Tags { get; set; } = null;
}
    