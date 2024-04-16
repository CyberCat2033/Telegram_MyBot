using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Commands.Goodbye;

public class SetGoodbyeCommand : TelegramBotCommands
{
    public SetGoodbyeCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancelationToken)
    {
        var settings = SettingsFactory.TryGet(message.Chat.Id);
        await settings.SetGoodbye(message);
        await MessageSenderAndDeleter.SendMessageAndDeleteAsync(message,
        botClient,
        cancelationToken,
        text: $"The godbye message has been successfullly changed");
    }
}
