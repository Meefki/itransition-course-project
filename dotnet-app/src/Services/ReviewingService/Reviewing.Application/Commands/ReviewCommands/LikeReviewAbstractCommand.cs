namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record LikeReviewAbstractCommand(
    string ReviewId,
    string UserId);