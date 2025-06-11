using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Queries.Authenticate;

public class AuthenticateCommand : IRequest<AuthenticateResponse?>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
