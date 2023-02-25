using Afonya.Bot.Interfaces.Services;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryService _categoryService;

    public UpdateCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryService.Update(request.Category);
        return Task.FromResult(result);
    }
}