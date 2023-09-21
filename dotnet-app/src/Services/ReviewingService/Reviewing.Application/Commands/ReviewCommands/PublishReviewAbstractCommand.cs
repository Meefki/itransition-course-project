namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record PublishReviewAbstractCommand(
    string Name,
    string AuthorUserId,
    string Content,
    string ShortDesc,
    string? ImageContentType,
    Stream? ImageInputStream,
    string SubjectId,
    string SubjectName,
    int SubjectGrade,
    IEnumerable<string> Tags);