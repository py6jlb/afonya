using Afonya.Api.Interfaces.Services;
using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.Authenticate;

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse?>
{

    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public AuthenticateCommandHandler(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }

    public Task<AuthenticateResponse?> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Task.FromResult<AuthenticateResponse?>(null);

        var user = _userRepository.GetByName(request.Username);
        if (user == null) return Task.FromResult<AuthenticateResponse?>(null);

        var success = _hashService.VerifyPassword(user.Password, request.Password);
        if (!success) return Task.FromResult<AuthenticateResponse?>(null);

        var dto = new UserDto(user.Id.ToString(), user.Login, user.IsAdmin);
        var result = new AuthenticateResponse(dto);
        return Task.FromResult<AuthenticateResponse?>(result);
    }
}
