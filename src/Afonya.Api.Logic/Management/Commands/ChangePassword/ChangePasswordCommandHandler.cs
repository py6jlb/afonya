using Afonya.Domain.Repositories;
using Afonya.Api.Logic.Services;
using MediatR;
using Shared.Contracts;
using Afonya.Api.Interfaces.Services;

namespace Afonya.Api.Logic.Management.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }
    public Task<UserDto?> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Password)) 
            return Task.FromResult<UserDto?>(null);

        var hash = _hashService.HashPassword(request.Password);
        var result = _userRepository.ChangePassword(request.Id, hash);
        return result == null ?
            Task.FromResult<UserDto?>(null) :
            Task.FromResult<UserDto?>(new UserDto(result.Id.ToString(), result.Login, result.IsAdmin));
    }
}
