using Afonya.Bot.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository.Get(request.Category.Id);
        category.SetIsActive(request.Category.IsActive);
        category.SetIcon(request.Category.Icon);
        category.SetName(request.Category.Name);
        category.SetHumanName(request.Category.HumanName);
        var result = _categoryRepository.Update(category);

        return Task.FromResult(request.Category);
    }
}