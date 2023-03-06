using Afonya.Bot.Logic.Commands.MoneyTransactions.UpdateMoneyTransaction;
using Afonya.Bot.Logic.Queries.GetMoneyTransactions;
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
            [FromQuery, SwaggerParameter("Начало периода")]DateTime? startDate, 
            [FromQuery, SwaggerParameter("Конец периода")]DateTime? endDate, 
            [FromQuery, SwaggerParameter("Пользователь")]string? user, 
            [FromQuery, SwaggerParameter("Код категории")]string? category, 
            [FromQuery, SwaggerParameter("Включать начало и конец периода")]bool include = true)
        {
            var filter = new MoneyTransactionFilter
            {
                IncludeDate = include,
                StartDate = startDate, 
                EndDate = endDate, 
                User = user, 
                Category = category
            };

            var data = await _mediator.Send(new GetMoneyTransactionsQuery { Filter = filter });
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
