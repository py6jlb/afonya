using Afonya.Bot.Interfaces.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryRepository.Update(request.Category);
        return Task.FromResult(result);
    }
}