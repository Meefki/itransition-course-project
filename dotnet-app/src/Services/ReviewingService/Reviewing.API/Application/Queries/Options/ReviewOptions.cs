namespace Reviewing.API.Application.Queries.Options;

public record ReviewOptions(
    string Name,
    string AuthorUserId,
    string Content,
    string ShortDesc,
    string? image,
    string? imageType,
    string SubjectId,
    string SubjectName,
    int SubjectGrade,
    IEnumerable<string> Tags,
    string? Id = null);
