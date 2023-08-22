using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.DomainEvents;

public record CommendAddedDomainEvent(CommentId CommentId) : IDomainEvent;