﻿using Comments.Domain.SeedWork.DomainEvents;

namespace Comments.Domain.DomainEvents;

public record CommentAddedDomainEvent(CommentId CommentId) : IDomainEvent;