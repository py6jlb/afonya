using Afonya.Bot.Logic.Api.MoneyTransaction.Commands.UpdateMoneyTransaction;
using Afonya.Bot.Logic.Api.MoneyTransaction.Queries.GetMoneyTransactions;
using Afonya.Bot.WebWorker.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Afonya.Bot.WebWorker.Controllers
{
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
        [BasicAuthAdmin]
        public async Task<IReadOnlyCollection<MoneyTransactionDto>> Get(
            [FromQuery, SwaggerParameter("Месяц")]int? month, 
            [FromQuery, SwaggerParameter("Год")]int? year, 
            [FromQuery, SwaggerParameter("Пользователь")]string? user, 
            [FromQuery, SwaggerParameter("Код категории")]string? category)
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
        [BasicAuthAdmin]
        public async Task<bool> Put(MoneyTransactionDto data)
        {
            var result = await _mediator.Send(new UpdateMoneyTransactionCommand { MoneyTransaction = data });
            return result;
        }
    }
}
