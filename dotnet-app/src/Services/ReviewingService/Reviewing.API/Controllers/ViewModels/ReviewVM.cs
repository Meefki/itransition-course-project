namespace Reviewing.API.Controllers.ViewModels;

public record ReviewVM(
    string Name,
    string AuthorUserId,
    string Content,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int Grade,
    IEnumerable<string> Tags,
    string? Id = null);