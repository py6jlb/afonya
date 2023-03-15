using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.SetCategory;

public class SetCategoryCommand : IRequest<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public long ChatId { get; set; }
    public string CallbackQueryId { get; set; }
    public SetCategoryCallbackData CallbackData { get; set; }
}