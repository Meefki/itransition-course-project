using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.DomainEvents;

public record CommentDeletedDomainEvent(CommentId CommandId) : IDomainEvent;