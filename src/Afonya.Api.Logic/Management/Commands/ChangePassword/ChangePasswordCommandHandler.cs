using Afonya.Domain.Repositories;
using Afonya.Api.Logic.Services;
using MediatR;
using Shared.Contracts;
using Afonya.Api.Interfaces.Services;
using Afonya.Domain.Exceptions;

namespace Afonya.Api.Logic.Management.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }
    public Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Password))
            throw new AfonyaErrorException("Неверные параметры запроса");

        var hash = _hashService.HashPassword(request.Password);
        var result = _userRepository.ChangePassword(request.Id, hash);
        return Task.FromResult(result);
    }
}
