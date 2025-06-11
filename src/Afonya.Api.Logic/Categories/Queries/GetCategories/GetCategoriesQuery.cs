using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IReadOnlyCollection<CategoryDto>>
{
    public bool All { get; set; }
}