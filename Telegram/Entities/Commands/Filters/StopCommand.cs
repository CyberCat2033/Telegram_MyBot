using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik;
using Telegramchik.Commands;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Commands.Filters;

public class StopCommand : TelegramBotCommands
{
    public StopCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public async override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        var settings = SettingsFactory.TryGet(message.Chat.Id);
        await settings.RemoveFilter(message);
        await MessageSenderAndDeleter.SendMessageAndDeleteAsync(message, botClient,
            CT, $"The filter \"{message.Text.Split()[1]}\" has been stopped");
    }
}
