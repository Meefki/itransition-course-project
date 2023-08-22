using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.SeedWork;

public abstract class Entity<T> : IEntity
{
    public Entity(EntityIdentifier<T> id)
    {
        Id = id;
        _domainEvents = new();
    }

    int? _requestedHashCode;

    public abstract EntityIdentifier<T> Id { get; init; }

    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly() ?? new List<IDomainEvent>().AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public bool IsTransient()
    {
        return Id is null || Id.Value is null;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Entity<T>)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        Entity<T> item = (Entity<T>)obj;

        if (item.IsTransient() || IsTransient())
            return false;
        else
            return item.Id == Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31;

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();
    }

    public static bool operator ==(Entity<T> left, Entity<T> right)
    {
        if (Equals(left, null))
            return Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity<T> left, Entity<T> right)
    {
        return !(left == right);
    }
}
