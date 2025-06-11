using System.Text;
using Afonya.Domain.Entities;
using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public class StatisticsQueryHandler : IRequestHandler<StatisticQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public StatisticsQueryHandler(ITelegramBotClient botClient,
        IMoneyTransactionRepository moneyTransactionRepository,
        ICategoryRepository categoryRepository)
    {
        _botClient = botClient;
        _moneyTransactionRepository = moneyTransactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(StatisticQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.Get();
        var result = _moneyTransactionRepository.Get(request.Month, request.Year, null, null).Select(x => new MoneyTransactionDto
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

        var month = $"{request.Month}".Length == 1 ? $"0{request.Month}" : $"{request.Month}";
        var message = PrepareStatistics(result, categories, month, request.Year);
        await _botClient.DeleteMessage(request.ChatId, request.OriginalMessageId, cancellationToken: cancellationToken);
        await _botClient.SendMessage(request.ChatId, message, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        return true;
    }

    private string PrepareStatistics(IEnumerable<MoneyTransactionDto> transactions, IEnumerable<Category> categories, string? month, int? year)
    {
        var sb = new StringBuilder();
        sb.Append("<b>Статистика за ");
        if (!string.IsNullOrWhiteSpace(month))
        {
            sb.Append($"{month}.");
        }
        if (!string.IsNullOrWhiteSpace(month))
        {
            sb.Append($"{year}:");
        }
        sb.Append("</b>");
        sb.AppendLine();
        var totalMinusRaw = transactions.Where(x => x.Sign == "-").Select(x => 0 - x.Value).Sum();
        var totalMinusValue = Math.Round(totalMinusRaw, 2);

        sb.AppendLine($"");
        sb.AppendLine($"<b>Всего потрачено: {totalMinusValue} руб</b>");
        foreach (var c in categories)
        {
            var categoryRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "-").Select(x => 0 - x.Value).Sum();
            var categoryValue = Math.Round(categoryRaw, 2);
            var percents = categoryValue / totalMinusValue * 100;
            if (categoryValue == 0)
            {
                sb.AppendLine($"{c.Icon} {c.HumanName}: <b>-</b>");
            }
            else
            {
                sb.AppendLine($"{c.Icon} {c.HumanName}: <b>{Math.Round(percents, 1)}%</b> (<i>{Math.Round(categoryValue, 2)} руб</i>)");
            }
        }

        var totalPlusRaw = transactions.Where(x => x.Sign == "+").Select(x => x.Value).Sum();
        var totalPlusValue = Math.Round(totalPlusRaw, 2);
        if (totalPlusValue != 0)
        {
            sb.AppendLine($"");
            sb.AppendLine($"<b>Всего получено: {totalPlusValue} руб</b>");
            foreach (var c in categories)
            {
                var categoryRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "+").Select(x => x.Value).Sum();
                var categoryValue = Math.Round(categoryRaw, 2);
                var percents = categoryValue / totalPlusValue * 100;
                if (categoryValue == 0)
                {
                    sb.AppendLine($"{c.Icon} {c.HumanName}: <b>-</b>");
                }
                else
                {
                    sb.AppendLine($"{c.Icon} {c.HumanName}: <b>{Math.Round(percents, 1)}%</b> (<i>{Math.Round(categoryValue, 2)} руб</i>)");
                }
            }
        }


        return sb.ToString();
    }
}
