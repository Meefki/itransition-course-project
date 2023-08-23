using MediatR;
using Reviewing.Application.Commands;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;

namespace Reviewing.API.Application.Commands;

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