using Reviewing.Domain.SeedWork.DomainEvents;

namespace Reviewing.Domain.SeedWork;

public interface IEntity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
}
