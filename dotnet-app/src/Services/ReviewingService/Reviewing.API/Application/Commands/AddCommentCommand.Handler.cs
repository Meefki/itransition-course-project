using MediatR;
using Reviewing.Application.Commands;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;

namespace Reviewing.API.Application.Commands;

public class AddCommentCommandHandler :
    AddCommentAbstractCommandHandler<AddCommentCommand>,
    IRequestHandler<AddCommentCommand, CommandResponse<string>>
{
    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        ILogger<AddCommentCommandHandler> logger)
        : base(commentRepository, logger)
    { }
}