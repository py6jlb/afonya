using Afonya.Bot.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        if (string.IsNullOrWhiteSpace(request.UserName)) return null;
        var result = _userRepository.GetByName(request.UserName);
        return result == null ? 
            Task.FromResult<UserDto?>(null) : 
            Task.FromResult<UserDto?>(new UserDto(result.Id.ToString(), result.Login));
    }
}