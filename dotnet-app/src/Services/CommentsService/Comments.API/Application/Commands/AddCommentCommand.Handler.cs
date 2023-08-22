using Comments.Application.Commands;
using Comments.Application.Repositories;
using Comments.Application.SeedWork;
using MediatR;

namespace Comments.API.Application.Commands;

public class AddCommentCommandHandler : 
    AddCommentAbstractCommandHandler<AddCommentCommand>, 
    IRequestHandler<AddCommentCommand, CommandResponse<string>>
{
    public AddCommentCommandHandler(
        ICommentRepository commentRepository, 
        ILogger<AddCommentAbstractCommandHandler<AddCommentCommand>> logger)
        : base(commentRepository, logger)
    {}
}