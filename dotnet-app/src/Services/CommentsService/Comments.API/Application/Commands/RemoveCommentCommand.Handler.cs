using Comments.Application.Commands;
using Comments.Application.SeedWork;
using MediatR;

namespace Comments.API.Application.Commands;

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