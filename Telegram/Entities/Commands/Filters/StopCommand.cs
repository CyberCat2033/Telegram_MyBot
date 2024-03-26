using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik;
using Telegramchik.Commands;

namespace Telegramchik.Commands.Filters;

public class StopCommand : TelegramCommands, IMessageSenderAndDeleter
{
    public StopCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public async override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        await FiltersGroup.Remove(message);
        await IMessageSenderAndDeleter.SendMessageAndDeleteAsync(message, botClient,CT, $"The filter \"{message.Text.Split()[1]}\" has been stopped");
    }
}
