using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Telegramchik.Commands;

public interface IMessageSenderAndDeleter
{
    public static async Task SendMessageAndDeleteAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken CT,
        string text
    )
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            cancellationToken: CT
        );
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, CT);
        await Task.Delay(1000);
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId + 1, CT);
    }
}
