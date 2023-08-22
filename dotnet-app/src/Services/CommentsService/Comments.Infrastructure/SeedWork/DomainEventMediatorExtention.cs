using Comments.Domain.SeedWork;
using Users.Application.SeedWork.Mediator;

namespace Comments.Infrastructure.SeedWork;

public static class DomainEventMediatorExtention
{
    public static async Task DispatchDomainEventsAsync(
        this IDomainEventMediator mediator,
        CommentDbContext context,
        CancellationToken cancellationToken = default)
    {
        var domainEntities = context.ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        List<Task> tasks = new();
        foreach (var domainEvent in domainEvents)
            tasks.Add(mediator.Publish(domainEvent, cancellationToken)!);

        await Task.WhenAll(tasks);
    }
}
