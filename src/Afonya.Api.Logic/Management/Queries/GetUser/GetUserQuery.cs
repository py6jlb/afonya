using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.GetUser;

public class GetUserQuery : IRequest<UserDto?>
{
    public string? UserName { get; set; }
    public bool IsAdmin { get; set; }
}