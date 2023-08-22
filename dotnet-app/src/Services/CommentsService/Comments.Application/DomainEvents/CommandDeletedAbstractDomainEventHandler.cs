using Comments.Application.SeedWork;
using Comments.Domain.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Comments.Application.DomainEvents;

public class CommandDeletedAbstractDomainEventHandler<TRequest>
    : NotificationHandler<TRequest>
    where TRequest : CommentDeletedDomainEvent
{
    public CommandDeletedAbstractDomainEventHandler(ILogger logger) 
        : base(logger)
    {
    }

    protected override Task Action(TRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}