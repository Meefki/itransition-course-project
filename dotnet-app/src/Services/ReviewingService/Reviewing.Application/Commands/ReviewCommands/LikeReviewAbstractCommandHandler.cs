using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands.ReviewCommands;

public class LikeReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : LikeReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;

    public LikeReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        ILogger logger) 
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
    }

    protected override async Task Action(TRequest request, CancellationToken cancellationToken)
    {
        ReviewId reviewId = ReviewId.Create<ReviewId>(Guid.Parse(request.ReviewId));
        Review review = await reviewRepository.GetById(reviewId);
        review.ChangeLike(new(Guid.Parse(request.UserId)));

        await reviewRepository.Update(review);
        await reviewRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);
    }
}