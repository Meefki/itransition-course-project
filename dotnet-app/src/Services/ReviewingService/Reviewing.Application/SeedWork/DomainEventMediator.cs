using Microsoft.Extensions.DependencyInjection;
using Reviewing.Domain.SeedWork.DomainEvents;
using System.Collections.Concurrent;
using System.Reflection;

namespace Reviewing.Application.SeedWork;

public class DomainEventMediator
    : IDomainEventMediator
{
    private readonly ConcurrentDictionary<Type, List<object>> handlers = new();
    //private readonly ConcurrentDictionary<Type, List<Type>> handlers = new();

    public DomainEventMediator(
        IServiceScopeFactory serviceScopeFactory,
        Type assemblyType)
    {
        var domainEventHandlerTypes = Assembly
                .GetAssembly(assemblyType)!
                .GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType &&
                         y.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)) &&
                       !x.IsAbstract);

        using var scope = serviceScopeFactory.CreateScope();
        foreach (var handlerType in domainEventHandlerTypes)
        {
            var domainEventType = handlerType
                .GetInterfaces()
                .First()
                .GetGenericArguments()
                .First();
            var registereMethod = typeof(DomainEventMediator)
                .GetMethod(nameof(DomainEventMediator.Register))!
                .MakeGenericMethod(domainEventType!);

            var handlerInstance = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handlerType)!;

            registereMethod.Invoke(this, new object[] { handlerInstance });
        }
    }

    public async Task Publish<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        var domainEventType = domainEvent.GetType();
        if (handlers.ContainsKey(domainEventType))
            foreach (var handler in handlers[domainEventType])
            {
                var handlerType = handler.GetType();
                var domainHandler = Convert.ChangeType(handler, handlerType);

                var handleMethod = handler
                    .GetType()
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle));

                var asyncMethod = (Task)handleMethod!.Invoke(handler, new object[] { domainEvent, cancellationToken })!;

                await asyncMethod;
            }
    }

    public void Register<T>(IDomainEventHandler<T> handler) where T : IDomainEvent
    {
        var domainEventType = typeof(T);
        if (!handlers.ContainsKey(domainEventType))
            handlers[domainEventType] = new();

        handlers[domainEventType].Add(handler);
    }
}
