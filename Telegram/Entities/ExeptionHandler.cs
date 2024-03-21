using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik
{
    public static class ExeptionHandler
    {
        private static ITelegramBotClient _botClient;
        private static Message _message;
        private static CancellationToken _token;

        public static async Task ChangeFields(Message message,ITelegramBotClient botClient, CancellationToken token)
        {
            await Task.Run(() =>
            {
                _botClient = botClient;
                _message = message;
                _token = token; 
            });
        }

        public static async Task SendExeptionMessageAsync(string exeptionMessage)
        {
            await _botClient.SendTextMessageAsync(
                chatId: _message.Chat.Id,
                text: exeptionMessage,
                replyToMessageId: _message.MessageId,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                cancellationToken: _token
                );
            await _botClient.DeleteMessageAsync(
                chatId: _message.Chat.Id,
                messageId: _message.MessageId,
                cancellationToken: _token
                );
        }
    }
}
