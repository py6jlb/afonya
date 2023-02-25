using Afonya.Bot.Interfaces.Services;
using Common.Exceptions;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.AddCategory;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    private readonly ICategoryService _categoryService;

    public AddCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryService.Create(request.NewCategory);
        if (result == null)
            throw new AfonyaErrorException("При создании категории, что-то пошло не так.");

        return Task.FromResult(result);
    }
}