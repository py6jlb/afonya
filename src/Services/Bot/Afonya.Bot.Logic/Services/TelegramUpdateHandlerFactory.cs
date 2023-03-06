using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Afonya.Bot.Logic.TelegramUpdateHandlers;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Services;

public class TelegramUpdateHandlerFactory : ITelegramUpdateHandlerFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly IBotKeyboardService _botKeyboard;
    private readonly ICallbackRepository _callbackRepository;

    public TelegramUpdateHandlerFactory(ILoggerFactory loggerFactory, 
        ITelegramBotClient botClient, IMoneyTransactionRepository moneyTransaction, 
        IBotKeyboardService botKeyboard, ICallbackRepository callbackRepository)
    {
        _loggerFactory = loggerFactory;
        _botClient = botClient;
        _moneyTransaction = moneyTransaction;
        _botKeyboard = botKeyboard;
        _callbackRepository = callbackRepository;
    }

    public ITelegramUpdateHandler CreateHandler(Update update)
    {
        return update switch
        {
            { Message: { } } => new MessageHandler(_loggerFactory.CreateLogger<MessageHandler>(), _botClient, _moneyTransaction, _botKeyboard),
            { EditedMessage: { } } => new EditedMessageHandler(_loggerFactory.CreateLogger<EditedMessageHandler>(), _botClient),
            { CallbackQuery: { } } => new CallbackQueryHandler(_loggerFactory.CreateLogger<CallbackQueryHandler>(), _botClient, _moneyTransaction, _botKeyboard, _callbackRepository),
            _ => new UnknownUpdateHandler(_loggerFactory.CreateLogger<UnknownUpdateHandler>(), _botClient)
        };
    }
}