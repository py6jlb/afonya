using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.Categories.AddCategory;

public class AddCategoryCommand : IRequest<CategoryDto>
{
    public CategoryDto NewCategory { get; set; }
}