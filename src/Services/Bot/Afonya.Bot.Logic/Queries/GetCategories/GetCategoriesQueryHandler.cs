using Afonya.Bot.Interfaces.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<IReadOnlyCollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.Get(request.All);
        var result = categories.Select(x => 
            new CategoryDto
            {
                Id = x.Id.ToString(), 
                Icon = x.Icon, 
                HumanName = x.HumanName, 
                IsActive = x.IsActive, 
                Name = x.Name
            }).ToList();
        return Task.FromResult<IReadOnlyCollection<CategoryDto>>(result);
    }
}