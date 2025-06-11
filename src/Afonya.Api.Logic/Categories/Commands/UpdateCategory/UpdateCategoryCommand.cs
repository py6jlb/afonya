using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public CategoryDto Category { get; set; }
}