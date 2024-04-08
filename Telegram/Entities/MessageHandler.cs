using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegramchik;

public class MessageHandler
{
    protected string? Text { get;  set; }
    protected MessageType Type { get;  set; }
    protected string? FileId { get;  set; }

    public MessageHandler(Message message)
    {
        if (message.ReplyToMessage == null)
        {
            throw new ArgumentException("You MUST reply to message");
        }
    }

    public MessageHandler() { }

    protected void ParseMessage(Message message)
    {
        FileId = GetFileId(message.ReplyToMessage);
        Type = message.ReplyToMessage.Type;
        Text = Type == MessageType.Text ? message.ReplyToMessage.Text : "";
    }

    protected virtual string? GetFileId(Message message)
    {
        return message.Type switch
        {
            MessageType.Sticker => message.Sticker.FileId,
            MessageType.Text => null,
            MessageType.Audio => message.Audio.FileId,
            MessageType.Photo => message.Photo[0].FileId,
            MessageType.Video => message.Video.FileId,
            MessageType.Voice => message.Voice.FileId,
            MessageType.VideoNote => message.VideoNote.FileId,
            _ => throw new ArgumentException("Unexpected message type", nameof(message)),
        };
    }

    public virtual async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ChatId chatId = message.Chat.Id;

        Dictionary<MessageType, Func<Task>> messageHandlers = new Dictionary<MessageType, Func<Task>>
        {
            { MessageType.Text, async () => await botClient.SendTextMessageAsync(chatId, Text, replyToMessageId: message.MessageId, parseMode: ParseMode.Html, cancellationToken: cancellationToken) },
            { MessageType.Photo, async () => await botClient.SendPhotoAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Audio, async () => await botClient.SendAudioAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Video, async () => await botClient.SendVideoAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Voice, async () => await botClient.SendVoiceAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.Sticker, async () => await botClient.SendStickerAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.VideoNote, async () => await botClient.SendVideoNoteAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        };

        if (messageHandlers.ContainsKey(Type))
        {
            await messageHandlers[Type].Invoke();
        }
        else
        {
            return;
        }
    }

}
