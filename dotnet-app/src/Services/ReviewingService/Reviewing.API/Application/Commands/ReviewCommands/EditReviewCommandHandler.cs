using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class EditReviewCommandHandler :
    EditReviewAbstractCommandHandler<EditReviewCommand>,
    IRequestHandler<EditReviewCommand, CommandResponse>
{
    public EditReviewCommandHandler(
        IReviewRepository reviewRepository, 
        ILogger<EditReviewCommandHandler> logger) 
        : base(reviewRepository, logger)
    { }
}