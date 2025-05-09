using Shared.Contracts;
using WebUI.Services.Interfaces;

namespace WebUI.Services;

public class OperationsService : IOperationsService
{

    private IHttpService _httpService;

    public OperationsService(
        IHttpService httpService
    )
    {
        _httpService = httpService;
    }
    public async Task<IEnumerable<MoneyTransactionDto>?> LoadOperations()
    {
        var data = await _httpService.Get<IEnumerable<MoneyTransactionDto>>("MoneyTransaction");
        return data;
    }
}
