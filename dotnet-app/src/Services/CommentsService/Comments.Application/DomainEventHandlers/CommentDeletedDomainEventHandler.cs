using Comments.Application.SeedWork;
using Comments.Domain.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Comments.Application.DomainEvents;

public class CommentDeletedDomainEventHandler
    : NotificationHandler<CommentDeletedDomainEvent>
{
    public CommentDeletedDomainEventHandler(
        ILogger<CommentDeletedDomainEventHandler> logger) 
        : base(logger)
    { }

    protected override async Task Action(CommentDeletedDomainEvent request, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
        await Task.CompletedTask;
    }
}