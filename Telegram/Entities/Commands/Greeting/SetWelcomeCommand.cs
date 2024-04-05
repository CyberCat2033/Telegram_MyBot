using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik.Commands;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Greeting;

public class SetWelcomeCommand : TelegramBotCommands
{
    public SetWelcomeCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancelationToken)
    {
        var settings = SettingsFactory.TryGet(message.Chat.Id);
        await settings.SetWelcome(message);
        await MessageSenderAndDeleter.SendMessageAndDeleteAsync(message,
        botClient,
        cancelationToken,
        text: $"The welcome message has been successfullly changed");
    }
}
