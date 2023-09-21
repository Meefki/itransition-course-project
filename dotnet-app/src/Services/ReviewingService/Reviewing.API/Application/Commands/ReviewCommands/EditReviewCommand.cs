using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record EditReviewCommand(
    string ReviewId,
    string Name,
    string Content,
    string ShortDesc,
    string? ImageContentType,
    Stream? ImageInputStream,
    string SubjectId,
    string SubjectName,
    int Grade,
    IEnumerable<string> Tags) : 
    EditReviewAbstractCommand(
        ReviewId,
        Name,
        Content,
        ShortDesc,
        ImageContentType,
        ImageInputStream,
        SubjectId,
        SubjectName,
        Grade,
        Tags), 
    IRequest<CommandResponse>;