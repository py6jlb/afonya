
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.GetUserById;

public class GetUserByIdQuery: IRequest<UserDto?>
{
    public string? UserId { get; set; }
}
