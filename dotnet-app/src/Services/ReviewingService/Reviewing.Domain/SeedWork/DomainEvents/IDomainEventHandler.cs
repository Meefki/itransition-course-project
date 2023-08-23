namespace Reviewing.Domain.SeedWork.DomainEvents;

public interface IDomainEventHandler<in DomainEvent>
    where DomainEvent : IDomainEvent
{
    Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
