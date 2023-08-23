using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands;

public abstract class AddCommentAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest, string>
    where TRequest : AddCommentAbstractCommand
{
    private readonly ICommentRepository commentRepository;

    public AddCommentAbstractCommandHandler(
        ICommentRepository commentRepository,
        ILogger logger)
        : base(logger)
    {
        this.commentRepository = commentRepository;
    }

    protected override async Task<string> Action(TRequest request, CancellationToken cancellationToken)
    {
        ReviewId reviewId = ReviewId.Create<ReviewId>(Guid.Parse(request.ReviewId));
        UserId userId = UserId.Create<UserId>(Guid.Parse(request.UserId));
        Comment comment = new(reviewId, userId, request.Text);
        await commentRepository.Add(comment);

        await commentRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        return comment.Id.ToString();
    }
}