﻿namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record EditReviewAbstractCommand(
    string ReviewId,
    string Name,
    string Content,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int Grade,
    IEnumerable<string> Tags);