using System;
using Afonya.Domain.Exceptions;
using Afonya.Domain.Repositories;
using MediatR;

namespace Afonya.Api.Logic.Management.Commands.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public EditUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<bool> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Login))
            throw new AfonyaErrorException("Неверные параметры запроса");

        var res = _userRepository.Update(request.Id, request.Login, request.IsAdmin);
        return Task.FromResult(res);
    }
}
