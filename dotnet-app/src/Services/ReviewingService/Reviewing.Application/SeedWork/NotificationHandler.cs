using Comments.Domain.DomainExceptions;
using Comments.Domain.SeedWork.DomainEvents;
using Comments.Domain.SeedWork.DomainExceptions;
using Microsoft.Extensions.Logging;

namespace Comments.Application.SeedWork;

public abstract class NotificationHandler<TRequest>
    : IDomainEventHandler<TRequest>
    where TRequest : IDomainEvent
{
    private readonly ILogger logger;

    public NotificationHandler(ILogger logger)
    {
        this.logger = logger;
    }

    protected abstract Task Action(TRequest request, CancellationToken cancellationToken);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public async Task Handle(TRequest domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            await Action(domainEvent, cancellationToken);
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, message: ex.Message);
        }
        catch (Exception ex)
        {
            SomethingWentWrongDomainException domainException = new(SomethingWentWrongDomainException.MessageText, ex);
            logger.LogError(domainException, message: SomethingWentWrongDomainException.MessageText);
            logger.LogCritical(ex, message: ex.Message);
        }
    }
}