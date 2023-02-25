using Afonya.Bot.Interfaces.Services;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryService _categoryService;

    public DeleteCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _categoryService.Delete(request.Id);
        return Task.FromResult(result);
    }
}