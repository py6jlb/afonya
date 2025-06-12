using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.GetUsers;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
}