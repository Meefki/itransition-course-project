using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork.DomainEvents;

namespace Reviewing.Domain.AggregateModels.CommentAggregate.DomainEvents;

public record CommentAddedDomainEvent(CommentId CommentId, ReviewId ReviewId) : IDomainEvent;