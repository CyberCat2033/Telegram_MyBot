using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands.Filters;

namespace Telegramchik;

public interface IMessageProperties
{
   string? Text { get;}
   MessageType Type { get;}
   string? FileId { get;}

    public async Task ExecuteAsync(IMessageProperties MesProp, Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ChatId chatId = message.Chat.Id;

        Dictionary<MessageType, Func<Task>> messageHandlers = new Dictionary<MessageType, Func<Task>>
        {
            { MessageType.Text, async () => await botClient.SendTextMessageAsync(chatId, MesProp.Text, replyToMessageId: message.MessageId, parseMode: ParseMode.Html, cancellationToken: cancellationToken) },
            { MessageType.Photo, async () => await botClient.SendPhotoAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Audio, async () => await botClient.SendAudioAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Video, async () => await botClient.SendVideoAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Voice, async () => await botClient.SendVoiceAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.Sticker, async () => await botClient.SendStickerAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.VideoNote, async () => await botClient.SendVideoNoteAsync(chatId, InputFile.FromFileId(MesProp.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        };

        if (messageHandlers.ContainsKey(MesProp.Type))
        {
            await messageHandlers[MesProp.Type].Invoke();
        }
        else
        {
            return;
        }
    }
}
