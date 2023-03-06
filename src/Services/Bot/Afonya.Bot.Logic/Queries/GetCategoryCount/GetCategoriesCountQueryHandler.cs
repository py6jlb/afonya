using Afonya.Bot.Interfaces.Repositories;
using MediatR;

namespace Afonya.Bot.Logic.Queries.GetCategoryCount;

public class GetCategoriesCountQueryHandler : IRequestHandler<GetCategoriesCountQuery, long>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesCountQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<long> Handle(GetCategoriesCountQuery request, CancellationToken cancellationToken)
    {
        var count = _categoryRepository.Count();
        return Task.FromResult<long>(count);
    }
}