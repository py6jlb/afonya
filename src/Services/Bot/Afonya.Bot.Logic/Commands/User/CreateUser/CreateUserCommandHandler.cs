using Afonya.Bot.Interfaces.Repositories;
using Common.Exceptions;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.User.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
        var result = _userRepository.Create(request.NewUser);
        if (result == null)
            throw new AfonyaErrorException("При создании пользователя, что-то пошло не так.");

        return Task.FromResult(result);
    }
}