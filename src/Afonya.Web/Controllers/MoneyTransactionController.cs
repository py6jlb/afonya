using Afonya.Api.Logic.MoneyTransaction.Commands.CreateMoneyTransaction;
using Afonya.Api.Logic.MoneyTransaction.Commands.UpdateMoneyTransaction;
using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransactions;
using Afonya.Web.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Afonya.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MoneyTransactionController : ControllerBase
{
    private readonly ILogger<MoneyTransactionController> _logger;
    private readonly IMediator _mediator;

    public MoneyTransactionController(ILogger<MoneyTransactionController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<MoneyTransactionDto>> Get(
        [FromQuery, SwaggerParameter("Месяц")] int? month,
        [FromQuery, SwaggerParameter("Год")] int? year,
        [FromQuery, SwaggerParameter("Пользователь")] string? user,
        [FromQuery, SwaggerParameter("Код категории")] string? category)
    {
        var data = await _mediator.Send(new GetMoneyTransactionsQuery
        {
            Month = month,
            Year = year,
            User = user,
            Category = category
        });
        return data;
    }

    [HttpPut]
    public async Task<bool> Put(MoneyTransactionDto data)
    {
        var result = await _mediator.Send(new UpdateMoneyTransactionCommand { MoneyTransaction = data });
        return result;
    }


    // [HttpPost]
    // public async Task<bool> Post(MoneyTransactionDto data)
    // {
    //     var result = await _mediator.Send(new CreateMoneyTransactionCommand { });
    //     return result;
    // }
}

