using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public CategoryDto Category { get; set; }
}