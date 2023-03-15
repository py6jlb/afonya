using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface ICommandBuilder
{
    IBaseRequest FromUpdate(Update update);
}