using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.PipelineBehaviors.Auth;

public class BotAuthBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : BaseBotCommand<TResponse>
{
    private readonly ILogger<BotAuthBehavior<TRequest, TResponse>> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _botClient;
    private readonly string _forbiddenMsg;
    public BotAuthBehavior(IUserRepository userRepository, 
        ILogger<BotAuthBehavior<TRequest, TResponse>> logger, 
        ITelegramBotClient botClient, IConfiguration config)
    {
        _userRepository = userRepository;
        _logger = logger;
        _botClient = botClient;
        _forbiddenMsg = config["Messages:Forbidden"] ?? "-";
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var allowed = !string.IsNullOrWhiteSpace(request.From) && _userRepository.GetByName(request.From) != null;
        if(allowed) return await next();
        await _botClient.SendMessage(chatId: request.ChatId, text: _forbiddenMsg, cancellationToken: cancellationToken);
        throw new AfonyaForbiddenException("Пользователь не авторизован");
    }
}