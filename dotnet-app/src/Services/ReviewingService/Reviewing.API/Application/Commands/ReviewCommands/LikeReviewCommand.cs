using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record LikeReviewCommand(string ReviewId, string UserId) : 
    LikeReviewAbstractCommand(ReviewId, UserId), 
    IRequest<CommandResponse>;
