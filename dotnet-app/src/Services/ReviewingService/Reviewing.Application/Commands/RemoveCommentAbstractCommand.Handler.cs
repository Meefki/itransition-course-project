using Comments.Application.Repositories;
using Comments.Application.SeedWork;
using Comments.Domain;
using Microsoft.Extensions.Logging;

namespace Comments.Application.Commands;

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
        Comment comment = await commentRepository.Get(id);
        comment.Delete();
        await commentRepository.Delete(comment);

        await commentRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);
    }
}