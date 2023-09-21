namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record EstimateReviewAbstractCommand(
    string ReviewId,
    string UserId,
    int Grade);