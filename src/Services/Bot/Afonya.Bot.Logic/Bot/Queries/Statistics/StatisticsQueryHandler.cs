using Afonya.Bot.Domain.Repositories;
using MediatR;
using Shared.Contracts;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public class StatisticsQueryHandler : IRequestHandler<StatisticQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public StatisticsQueryHandler(ITelegramBotClient botClient, IMoneyTransactionRepository moneyTransactionRepository)
    {
        _botClient = botClient;
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public async Task<bool> Handle(StatisticQuery request, CancellationToken cancellationToken)
    {
        var result = _moneyTransactionRepository.Get(request.Month, request.Year,
           request.User, request.Category).Select(x => new MoneyTransactionDto
           {
               Id = x.Id?.ToString(),
               CategoryName = x.CategoryName,
               CategoryHumanName = x.CategoryHumanName,
               CategoryIcon = x.CategoryIcon,
               Value = x.Value,
               Sign = x.Sign,
               RegisterDate = x.RegisterDate,
               TransactionDate = x.TransactionDate,
               FromUserName = x.FromUserName
           }).ToArray();

        //throw new NotImplementedException();
        
        await _botClient.SendMessage(chatId: request.ChatId, text: "-------", cancellationToken: cancellationToken);
        return true;
    }
}
