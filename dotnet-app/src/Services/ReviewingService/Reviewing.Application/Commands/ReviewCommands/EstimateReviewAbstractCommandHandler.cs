using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class EstimateReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : EstimateReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;

    protected EstimateReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        ILogger logger) 
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
    }

    protected override async Task Action(TRequest request, CancellationToken cancellationToken)
    {
        ReviewId reviewId = ReviewId.Create<ReviewId>(Guid.Parse(request.ReviewId));
        UserId userId = UserId.Create<UserId>(Guid.Parse(request.UserId));
        Review review = await reviewRepository.GetById(reviewId);
        Estimate estimate = new(userId, request.Grade);
        review.ChangeEstimate(estimate);

        await reviewRepository.Update(review);
        await reviewRepository.UnitOfWork
            .SaveChangesAsync(cancellationToken);
    }
}