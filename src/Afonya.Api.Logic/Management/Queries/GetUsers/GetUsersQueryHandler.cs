using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Queries.GetUsers;

public class GetUserQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = _userRepository.Get();
        return Task.FromResult(result.Select(x => new UserDto(x.Id.ToString(), x.Login, x.IsAdmin)));
    }
}