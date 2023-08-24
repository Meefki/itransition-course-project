using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class DeleteReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : DeleteReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;

    protected DeleteReviewAbstractCommandHandler(
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
        review.ChangeStatus(ReviewStatuses.Deleted);

        await reviewRepository.Update(review);
        await reviewRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);
    }
}