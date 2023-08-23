using Microsoft.Extensions.DependencyInjection;
using Reviewing.Domain.SeedWork.DomainEvents;
using System.Collections.Concurrent;
using System.Reflection;

namespace Reviewing.Application.SeedWork;

public class DomainEventMediator
    : IDomainEventMediator
{
    private readonly ConcurrentDictionary<Type, List<Type>> handlers = new();
    private readonly IServiceScopeFactory serviceScopeFactory;

    public DomainEventMediator(
        IServiceScopeFactory serviceScopeFactory,
        Type assemblyType)
    {
        this.serviceScopeFactory = serviceScopeFactory;

        var domainEventHandlerTypes = Assembly
                .GetAssembly(assemblyType)!
                .GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType &&
                         y.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)) &&
                       !x.IsAbstract);

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

            registereMethod.Invoke(this, new object[] { handlerType });
        }
    }

    public async Task Publish<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        var domainEventType = domainEvent.GetType();
        if (handlers.ContainsKey(domainEventType))
        {
            using var scope = serviceScopeFactory.CreateScope();
            foreach (var handlerType in handlers[domainEventType])
            {
                var handlerInstance = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handlerType)!;

                var handleMethod = handlerInstance
                    .GetType()
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle));

                var asyncMethod = (Task)handleMethod!.Invoke(handlerInstance, new object[] { domainEvent, cancellationToken })!;

                await asyncMethod;
            }
        }
    }

    public void Register<T>(Type handlerType)
        where T : IDomainEvent
    {
        if (!handlers.ContainsKey(handlerType))
            handlers[typeof(T)] = new();

        handlers[typeof(T)].Add(handlerType);
    }
}
