using Afonya.Bot.Interfaces.Services;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.BotStop;

public class BotStopCommandHandler : IRequestHandler<BotStopCommand, bool>
{
    private readonly IBotManagementService _botManagementService;

    public BotStopCommandHandler(IBotManagementService botManagementService)
    {
        _botManagementService = botManagementService;
    }

    public async Task<bool> Handle(BotStopCommand request, CancellationToken cancellationToken)
    {
        await _botManagementService.StopAsync(cancellationToken);
        return true;
    }
}