using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class LikeReviewCommandHandler
    : LikeReviewAbstractCommandHandler<LikeReviewCommand>,
    IRequestHandler<LikeReviewCommand, CommandResponse>
{
    public LikeReviewCommandHandler(
        IReviewRepository reviewRepository, 
        ILogger<LikeReviewCommandHandler> logger) 
        : base(reviewRepository, logger)
    { }
}