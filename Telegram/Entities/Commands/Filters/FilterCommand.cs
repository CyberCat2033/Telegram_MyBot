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
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Filter \"{message.Text.Split()[1]}\" has been added",
            cancellationToken: CT
            );
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, CT);
        await Task.Delay(1000);
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId + 1, CT);
    }
}

