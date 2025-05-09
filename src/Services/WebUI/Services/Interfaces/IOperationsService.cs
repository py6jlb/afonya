using Shared.Contracts;

namespace WebUI.Services.Interfaces;

public interface IOperationsService
{
    public Task<IEnumerable<MoneyTransactionDto>?> LoadOperations();
}
