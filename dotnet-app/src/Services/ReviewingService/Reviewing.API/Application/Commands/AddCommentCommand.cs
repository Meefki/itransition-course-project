using Comments.Application.Commands;
using Comments.Application.SeedWork;
using MediatR;

namespace Comments.API.Application.Commands;

public record AddCommentCommand(string ReviewId, string UserId, string Text) : 
    AddCommentAbstractCommand(ReviewId, UserId, Text), 
    IRequest<CommandResponse<string>>;
