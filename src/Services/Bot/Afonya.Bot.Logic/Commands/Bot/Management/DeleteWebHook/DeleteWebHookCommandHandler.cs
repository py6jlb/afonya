using Afonya.Bot.Interfaces.Services;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Management.DeleteWebHook;

public class DeleteWebHookCommandHandler : IRequestHandler<DeleteWebHookCommand, bool>
{
    private readonly IBotManagementService _botManagementService;

    public DeleteWebHookCommandHandler(IBotManagementService botManagementService)
    {
        _botManagementService = botManagementService;
    }

    public async Task<bool> Handle(DeleteWebHookCommand request, CancellationToken cancellationToken)
    {
        await _botManagementService.StopAsync(cancellationToken);
        return true;
    }
}