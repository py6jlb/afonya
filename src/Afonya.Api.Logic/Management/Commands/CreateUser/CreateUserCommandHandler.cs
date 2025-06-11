using Afonya.Domain.Entities;
using Afonya.Domain.Exceptions;
using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;
using Afonya.Api.Interfaces.Services;

namespace Afonya.Api.Logic.Management.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public CreateUserCommandHandler(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }

    public Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existUser = _userRepository.GetByName(request.Login);
        if (existUser != null)
            throw new AfonyaErrorException("Пользователь уже существует.");

        var hash = _hashService.HashPassword(request.Password);
        var user = new User(request.Login, hash, request.IsAdmin);
        var result = _userRepository.Create(user)
            ?? throw new AfonyaErrorException("При создании пользователя, что-то пошло не так.");
        return Task.FromResult(new UserDto(result.Id.ToString(), result.Login, result.IsAdmin));
    }
}