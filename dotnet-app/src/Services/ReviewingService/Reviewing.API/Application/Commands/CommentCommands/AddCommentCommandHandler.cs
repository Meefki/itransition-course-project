﻿using MediatR;
using Reviewing.Application.Commands.CommentCommands;

namespace Reviewing.API.Application.Commands.CommentCommands;

public class AddCommentCommandHandler :
    AddCommentAbstractCommandHandler<AddCommentCommand>,
    IRequestHandler<AddCommentCommand, CommandResponse<string>>
{
    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        ILogger<AddCommentCommandHandler> logger)
        : base(commentRepository, logger)
    { }
}