using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Categories.Commands.AddCategory;

public class AddCategoryCommand : IRequest<CategoryDto>
{
    public CategoryDto NewCategory { get; set; }
}