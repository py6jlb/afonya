using Afonya.Bot.Interfaces.Services;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
{
    private readonly IUserService _userService;

    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        if (string.IsNullOrWhiteSpace(request.UserName)) return null;
        var result = _userService.GetByName(request.UserName);
        return Task.FromResult(result);
    }
}