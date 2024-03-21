using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands.Filters;

namespace Telegramchik.Commands;

public class FilterCommand : TelegramCommands
{
    public FilterCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public async override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        await FiltersGroup.Add(message);
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, CT);
    }
}

