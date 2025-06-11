using Afonya.Bot.Interfaces.Services;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Delegates;

public delegate ICommandBuilder CommandBuilderResolver(UpdateType type);