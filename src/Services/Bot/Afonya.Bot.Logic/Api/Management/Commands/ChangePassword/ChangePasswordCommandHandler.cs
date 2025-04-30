using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Logic.Services;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public ChangePasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<UserDto?> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Password)) 
            return Task.FromResult<UserDto?>(null);

        var hash = HashService.HashPassword(request.Password);
        var result = _userRepository.ChangePassword(request.Id, hash);
        return result == null ?
            Task.FromResult<UserDto?>(null) :
            Task.FromResult<UserDto?>(new UserDto(result.Id.ToString(), result.Login));
    }
}
