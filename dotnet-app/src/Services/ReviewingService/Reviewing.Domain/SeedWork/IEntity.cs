using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.SeedWork;

public interface IEntity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
}
