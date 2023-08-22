using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.DomainEvents;

public record CommandDeletedDomainEvent(CommentId CommandId) : IDomainEvent;