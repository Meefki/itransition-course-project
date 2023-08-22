﻿using Comments.Domain.SeedWork.DomainEvents;

namespace Users.Application.SeedWork.Mediator;

public interface IDomainEventMediator
{
    void Register<T>(IDomainEventHandler<T> handler) where T : IDomainEvent;
    Task Publish<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
