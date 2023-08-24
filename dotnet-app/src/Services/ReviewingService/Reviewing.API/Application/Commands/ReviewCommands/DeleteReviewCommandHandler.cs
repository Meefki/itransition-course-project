using MediatR;

namespace Reviewing.Application.Commands.ReviewCommands;

public class DeleteReviewCommandHandler : 
    DeleteReviewAbstractCommandHandler<DeleteReviewCommand>,
    IRequestHandler<DeleteReviewCommand, CommandResponse>

{
    protected DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        ILogger<DeleteReviewCommandHandler> logger) 
        : base(reviewRepository, logger)
    { }
}