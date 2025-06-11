using MediatR;

namespace Afonya.Api.Logic.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public string Id { get; set; }
}