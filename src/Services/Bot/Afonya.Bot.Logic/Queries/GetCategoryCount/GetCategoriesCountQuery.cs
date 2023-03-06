using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetCategoryCount;

public class GetCategoriesCountQuery : IRequest<long>
{
}