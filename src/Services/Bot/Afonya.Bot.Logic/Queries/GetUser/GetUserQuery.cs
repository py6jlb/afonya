﻿using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetUser;

public class GetUserQuery : IRequest<UserDto?>
{
    public string? UserName { get; set; }
}