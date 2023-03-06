using MediatR;

namespace Afonya.Bot.Logic.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public string Id { get; set; }
}