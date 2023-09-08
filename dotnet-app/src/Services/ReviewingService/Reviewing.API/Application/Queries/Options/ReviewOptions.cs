namespace Reviewing.API.Application.Queries.Options;

public record ReviewOptions(
    string Name,
    string AuthorUserId,
    string Content,
    string ShortDesc,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int SubjectGrade,
    IEnumerable<string> Tags,
    string? Id = null);
