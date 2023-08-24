using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class DeleteReviewCommandHandler :
    DeleteReviewAbstractCommandHandler<DeleteReviewCommand>,
    IRequestHandler<DeleteReviewCommand, CommandResponse>

{
    public DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        ILogger<DeleteReviewCommandHandler> logger)
        : base(reviewRepository, logger)
    { }
}