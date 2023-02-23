﻿using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Logic.Commands.Bot.BotStop;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.BotStart;

public class BotStartCommandHandler : IRequestHandler<BotStopCommand, bool>
{
    private readonly IBotManagementService _botManagementService;

    public BotStartCommandHandler(IBotManagementService botManagementService)
    {
        _botManagementService = botManagementService;
    }

    public async Task<bool> Handle(BotStopCommand request, CancellationToken cancellationToken)
    {
        await _botManagementService.StartAsync(cancellationToken);
        return true;
    }
}