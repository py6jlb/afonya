using MediatR;

namespace Afonya.Bot.Logic.Commands.User.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}