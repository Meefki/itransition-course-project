using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record PublishReviewCommand(
    string Name,
    string AuthorUserId,
    string Content,
    string ShortDesc,
    string? ImageContentType,
    Stream? ImageInputStream,
    string SubjectId,
    string SubjectName,
    int SubjectGrade,
    IEnumerable<string> Tags) : 
    PublishReviewAbstractCommand(
        Name,
        AuthorUserId,
        Content,
        ShortDesc,
        ImageContentType,
        ImageInputStream,
        SubjectId,
        SubjectName,
        SubjectGrade,
        Tags),
    IRequest<CommandResponse<string>>;