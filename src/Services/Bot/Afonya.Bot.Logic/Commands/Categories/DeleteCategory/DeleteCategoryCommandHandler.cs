using Afonya.Bot.Interfaces.Repositories;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryRepository.Delete(request.Id);
        return Task.FromResult(result);
    }
}