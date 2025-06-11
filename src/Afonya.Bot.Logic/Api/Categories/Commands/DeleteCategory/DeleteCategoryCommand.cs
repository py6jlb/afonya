using MediatR;

namespace Afonya.Bot.Logic.Api.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public string Id { get; set; }
}