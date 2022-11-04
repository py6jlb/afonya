using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bot.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoneyTransactionController : ControllerBase
    {
        private readonly ILogger<MoneyTransactionController> _logger;
        private readonly IMoneyTransactionService _moneyTransaction;

        public MoneyTransactionController(ILogger<MoneyTransactionController> logger, 
            IMoneyTransactionService moneyTransaction)
        {
            _logger = logger;
            _moneyTransaction = moneyTransaction;
        }

        [HttpGet]
        
        public ActionResult<IEnumerable<MoneyTransactionDto>> Get(
            [FromQuery, SwaggerParameter("Начало периода")]DateTime? startDate, 
            [FromQuery, SwaggerParameter("Конец периода")]DateTime? endDate, 
            [FromQuery, SwaggerParameter("Пользователь")]string? user, 
            [FromQuery, SwaggerParameter("Код категории")]string? category, 
            [FromQuery, SwaggerParameter("Включать начало и конец периода")]bool include = false)
        {
            var filter = new MoneyTransactionFilter
            {
                IncludeDate = include,
                StartDate = startDate, 
                EndDate = endDate, 
                User = user, 
                Category = category
            };

            var data = _moneyTransaction.Get(filter).Select(x=> new MoneyTransactionDto
            {
                Id = x.Id.ToString(), 
                CategoryName = x.CategoryName, 
                CategoryHumanName = x.CategoryHumanName, 
                CategoryIcon = x.CategoryIcon, 
                Value = x.Value, 
                Sign = x.Sign, 
                RegisterDate = x.RegisterDate, 
                TransactionDate = x.TransactionDate, 
                FromUserName = x.FromUserName
            });
            return Ok(data);
        }

        [HttpPut]
        public ActionResult<bool> Put(MoneyTransactionDto data)
        {
            if (string.IsNullOrWhiteSpace(data.Id))
                return BadRequest("Отсуствует id записи для обновления.");

            var entity = _moneyTransaction.Get(data.Id);
            if (entity == null) return NotFound("Запись для обновления отсуствует");

            entity.Value = data.Value;
            entity.Sign = data.Sign;
            entity.CategoryName = data.CategoryName;
            entity.CategoryHumanName = data.CategoryHumanName;
            entity.CategoryIcon = data.CategoryIcon;
            entity.RegisterDate = data.RegisterDate;
            entity.TransactionDate = data.TransactionDate;
            entity.FromUserName = data.FromUserName;

            var res = _moneyTransaction.Update(entity);
            return Ok(res);
        }
    }
}
