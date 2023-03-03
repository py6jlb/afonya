using Afonya.Bot.Interfaces.Repositories;
using MediatR;

namespace Afonya.Bot.Logic.Commands.User.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = _userRepository.Delete(request.Id);
        return Task.FromResult(result);
    }
}