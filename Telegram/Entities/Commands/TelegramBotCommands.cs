using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public abstract class TelegramBotCommands : BotCommand
{

    public TelegramBotCommands(string Command, string Description = "")
    {
        this.Command = Command;
        this.Description = Description;
    }

    public abstract Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT);

}
