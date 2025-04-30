using Afonya.Bot.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) return Task.FromResult<UserDto?>(null);
        var result = _userRepository.Get(request.UserId);
        return result == null ?
            Task.FromResult<UserDto?>(null) :
            Task.FromResult<UserDto?>(new UserDto(result.Id.ToString(), result.Login));
    }
}
