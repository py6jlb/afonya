using Afonya.Bot.Interfaces.Repositories;
using Common.Exceptions;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.AddCategory;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public AddCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryRepository.Create(request.NewCategory);
        if (result == null)
            throw new AfonyaErrorException("При создании категории, что-то пошло не так.");

        return Task.FromResult(result);
    }
}