using Afonya.Bot.Interfaces.Services;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryDto>>
{
    private readonly ICategoryService _categoryService;

    public GetCategoriesQueryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<IReadOnlyCollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = _categoryService.Get(request.All);
        return Task.FromResult(result);
    }
}