using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork.DomainEvents;

namespace Reviewing.Domain.AggregateModels.CommentAggregate.DomainEvents;

public record CommentDeletedDomainEvent(CommentId CommandId, ReviewId ReviewId) : IDomainEvent;