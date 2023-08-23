using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands;

public abstract class RemoveCommentAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : RemoveCommentAbstractCommand
{
    private readonly ICommentRepository commentRepository;

    protected RemoveCommentAbstractCommandHandler(
        ICommentRepository commentRepository,
        ILogger logger)
        : base(logger)
    {
        this.commentRepository = commentRepository;
    }

    protected override async Task Action(TRequest request, CancellationToken cancellationToken)
    {
        CommentId id = CommentId.Create<CommentId>(Guid.Parse(request.CommentId));
        Comment comment = await commentRepository.GetById(id);
        comment.Delete();
        await commentRepository.Delete(comment);

        await commentRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);
    }
}