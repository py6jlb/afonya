using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Afonya.Bot.Logic.TelegramUpdateHandlers;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using IUpdateHandler = Afonya.Bot.Interfaces.Services.UpdateHandler.IUpdateHandler;

namespace Afonya.Bot.Logic.Services;

public class UpdateHandlerFactory : IUpdateHandlerFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateHandlerFactory(ILoggerFactory loggerFactory, ITelegramBotClient botClient, IMoneyTransactionRepository moneyTransaction, ICategoryRepository categoryRepository)
    {
        _loggerFactory = loggerFactory;
        _botClient = botClient;
        _moneyTransaction = moneyTransaction;
        _categoryRepository = categoryRepository;
    }

    public IUpdateHandler CreateHandler(Update update)
    {
        return update switch
        {
            { Message: { } } => new MessageHandler(_loggerFactory.CreateLogger<MessageHandler>(), _botClient, _moneyTransaction, _categoryRepository),
            { EditedMessage: { } } => new EditedMessageHandler(_loggerFactory.CreateLogger<EditedMessageHandler>(), _botClient),
            { CallbackQuery: { } } => new CallbackQueryHandler(_loggerFactory.CreateLogger<CallbackQueryHandler>(), _botClient, _moneyTransaction, _categoryRepository),
            _ => new UnknownUpdateHandler(_loggerFactory.CreateLogger<UnknownUpdateHandler>(), _botClient)
        };
    }
}