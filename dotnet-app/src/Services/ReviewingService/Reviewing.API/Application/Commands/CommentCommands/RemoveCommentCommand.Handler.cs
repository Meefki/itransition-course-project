using MediatR;
using Reviewing.Application.Commands.CommentCommands;

namespace Reviewing.API.Application.Commands.CommentCommands;

public class RemoveCommentCommandHandler :
    RemoveCommentAbstractCommandHandler<RemoveCommentCommand>,
    IRequestHandler<RemoveCommentCommand, CommandResponse>
{
    public RemoveCommentCommandHandler(
        ICommentRepository commentRepository,
        ILogger<RemoveCommentCommandHandler> logger)
        : base(commentRepository, logger)
    { }
}