using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public CategoryDto Category { get; set; }
}