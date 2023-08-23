using Microsoft.Extensions.Logging;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate.DomainEvents;

namespace Reviewing.Application.DomainEventHandlers;

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