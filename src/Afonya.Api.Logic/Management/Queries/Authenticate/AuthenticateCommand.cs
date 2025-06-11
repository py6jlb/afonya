using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.Authenticate;

public class AuthenticateCommand : IRequest<AuthenticateResponse?>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
