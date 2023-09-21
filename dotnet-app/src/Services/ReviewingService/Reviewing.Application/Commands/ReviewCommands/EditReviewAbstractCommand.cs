namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record EditReviewAbstractCommand(
    string ReviewId,
    string Name,
    string Content,
    string ShortDesc,
    string? ImageContentType,
    Stream? ImageInputStream,
    string SubjectId,
    string SubjectName,
    int SubjectGrade,
    IEnumerable<string> Tags);