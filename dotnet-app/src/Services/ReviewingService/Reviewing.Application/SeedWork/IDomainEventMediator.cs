using Reviewing.Domain.SeedWork.DomainEvents;

namespace Reviewing.Application.SeedWork;

public interface IDomainEventMediator
{
    Task Publish<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
    void Register<T>(Type handlerType) where T : IDomainEvent;
}
