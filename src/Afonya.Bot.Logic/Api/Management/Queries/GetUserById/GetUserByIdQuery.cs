
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Queries.GetUserById;

public class GetUserByIdQuery: IRequest<UserDto?>
{
    public string? UserId { get; set; }
}
