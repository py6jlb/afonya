using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Interfaces.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Categories.Commands.AddCategory;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public AddCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(
            request.NewCategory.Icon,
            request.NewCategory.Name,
            request.NewCategory.HumanName,
            request.NewCategory.IsActive
        );
        var result = _categoryRepository.Create(category);
        if (result == null)
            throw new AfonyaErrorException("При создании категории, что-то пошло не так.");

        return Task.FromResult(new CategoryDto
        {
            Id = result.Id.ToString(),
            Icon = result.Icon,
            HumanName = result.HumanName,
            IsActive = result.IsActive,
            Name = result.Name
        });
    }
}