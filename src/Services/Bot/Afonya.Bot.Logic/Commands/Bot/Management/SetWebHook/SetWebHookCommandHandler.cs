using Afonya.Bot.Interfaces.Services;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Management.SetWebHook;

public class SetWebHookCommandHandler : IRequestHandler<SetWebHookCommand, bool>
{
    private readonly IBotManagementService _botManagementService;

    public SetWebHookCommandHandler(IBotManagementService botManagementService)
    {
        _botManagementService = botManagementService;
    }

    public async Task<bool> Handle(SetWebHookCommand request, CancellationToken cancellationToken)
    {
        await _botManagementService.StartAsync(cancellationToken);
        return true;
    }
}