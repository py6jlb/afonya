using Afonya.Bot.Interfaces.Services;
using MediatR;

namespace Afonya.Bot.Logic.Commands.User.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = _userService.Delete(request.Id);
        return Task.FromResult(result);
    }
}