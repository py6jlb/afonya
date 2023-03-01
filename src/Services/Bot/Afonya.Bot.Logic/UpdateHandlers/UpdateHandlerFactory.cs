using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using IUpdateHandler = Afonya.Bot.Interfaces.Services.UpdateHandler.IUpdateHandler;

namespace Afonya.Bot.Logic.UpdateHandlers;

public class UpdateHandlerFactory : IUpdateHandlerFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionService _moneyTransaction;
    private readonly ICategoryService _categoryService;

    public UpdateHandlerFactory(ILoggerFactory loggerFactory, ITelegramBotClient botClient, IMoneyTransactionService moneyTransaction, ICategoryService categoryService)
    {
        _loggerFactory = loggerFactory;
        _botClient = botClient;
        _moneyTransaction = moneyTransaction;
        _categoryService = categoryService;
    }

    public IUpdateHandler CreateHandler(Update update)
    {
        return update switch
        {
            { Message: { } }            => new MessageHandler(_loggerFactory.CreateLogger<MessageHandler>(), _botClient, _moneyTransaction, _categoryService),
            { EditedMessage: { } }      => new EditedMessageHandler(_loggerFactory.CreateLogger<EditedMessageHandler>(), _botClient),
            { CallbackQuery: { } }      => new CallbackQueryHandler(_loggerFactory.CreateLogger<CallbackQueryHandler>(), _botClient, _moneyTransaction, _categoryService),
            _                           => new UnknownUpdateHandler(_loggerFactory.CreateLogger<UnknownUpdateHandler>(), _botClient)
        };
    }
}