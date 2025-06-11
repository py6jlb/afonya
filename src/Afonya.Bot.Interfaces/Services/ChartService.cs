using System;
using Afonya.Bot.Domain.Entities;
using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Services;

public interface IChartService
{
    Task<string> GetStatisticPng(string title, IEnumerable<MoneyTransactionDto> transactions, IEnumerable<Category> categories);
}
