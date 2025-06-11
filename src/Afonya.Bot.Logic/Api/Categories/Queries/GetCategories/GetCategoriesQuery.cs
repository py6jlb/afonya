using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IReadOnlyCollection<CategoryDto>>
{
    public bool All { get; set; }
}