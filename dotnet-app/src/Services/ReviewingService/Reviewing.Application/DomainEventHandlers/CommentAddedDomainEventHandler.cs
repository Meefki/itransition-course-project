using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate.DomainEvents;
using Reviewing.Domain.AggregateModels.ReviewAggregate;

namespace Reviewing.Application.DomainEventHandlers;

public class CommentAddedDomainEventHandler
    : NotificationHandler<CommentAddedDomainEvent>
{
    private readonly IReviewRepository reviewRepository;

    public CommentAddedDomainEventHandler(
        IReviewRepository reviewRepository,
        ILogger<CommentAddedDomainEventHandler> logger)
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
    }

    protected override async Task Action(CommentAddedDomainEvent request, CancellationToken cancellationToken)
    {
        Review review = await reviewRepository.GetById(request.ReviewId);
        review.AddComment(request.CommentId);

        await reviewRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);
    }
}