using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public static class MessageSenderAndDeleter
{
    public static async Task SendMessageAndDeleteAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancelationToken,
        string text,
        bool DeleteSelfMessage = true
    )
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            cancellationToken: cancelationToken
        );
        await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, cancelationToken);
        await Task.Delay(1000);
        if (DeleteSelfMessage) await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId + 1, cancelationToken);
    }
}
