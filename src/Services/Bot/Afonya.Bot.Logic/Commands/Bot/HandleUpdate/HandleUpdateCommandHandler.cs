using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Commands.Bot.HandleUpdate;

public class HandleUpdateCommandHandler : IRequestHandler<HandleUpdateCommand, bool>
{
    private readonly IUserService _userService;
    private readonly ITelegramUpdateService _updateService;

    public HandleUpdateCommandHandler(IUserService userService, ITelegramUpdateService updateService)
    {
        _userService = userService;
        _updateService = updateService;
    }

    public async Task<bool> Handle(HandleUpdateCommand request, CancellationToken cancellationToken)
    {
        var allowed = AllowedUser(request.Update);
        if (!allowed) return false;
        
        await _updateService.HandleUpdateAsync(request.Update, cancellationToken);
        return true;
    }

    private bool AllowedUser(Update? update)
    {
        if(update == null) return false;
        var from = update switch
        {
            { Message: { } message }                       => message?.From?.Username,
            { EditedMessage: { } message }                 => message?.From?.Username,
            { CallbackQuery: { } callbackQuery }           => callbackQuery?.From?.Username,
            { InlineQuery: { } inlineQuery }               => inlineQuery?.From?.Username,
            { ChosenInlineResult: { } chosenInlineResult } => chosenInlineResult?.From?.Username,
            _                                              => null
        };

        return !string.IsNullOrWhiteSpace(from) && _userService.GetByName(from) != null;
    }
}