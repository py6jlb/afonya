using Afonya.Bot.Interfaces.Services;
using Common.Exceptions;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.User.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
        var result = _userService.Create(request.NewUser);
        if (result == null)
            throw new AfonyaErrorException("При создании пользователя, что-то пошло не так.");

        return Task.FromResult(result);
    }
}