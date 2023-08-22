using Comments.Application.SeedWork;
using Comments.Domain.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Comments.Application.DomainEvents;

public class CommentAddedDomainEventHandler
    : NotificationHandler<CommentAddedDomainEvent>
{
    public CommentAddedDomainEventHandler(
        ILogger<CommentAddedDomainEventHandler> logger) 
        : base(logger)
    { }

    protected override async Task Action(CommentAddedDomainEvent request, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
        await Task.CompletedTask;
    }
}