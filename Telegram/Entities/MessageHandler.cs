using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands;

namespace Telegramchik;

public class MessageHandler
{
    #region  Properties and fields

    private static Dictionary<MessageType, Func<Message, InputFileId, ITelegramBotClient, CancellationToken, Task>> messageHandlers = new()

{

    { MessageType.Text, async (message, _, botClient, cancellationToken) =>
    await botClient.SendTextMessageAsync(message.Chat.Id, message.Text, replyToMessageId: message.MessageId, parseMode: ParseMode.Html, cancellationToken: cancellationToken) },

    { MessageType.Photo, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendPhotoAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

    { MessageType.Audio, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendAudioAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

    { MessageType.Video, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendVideoAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

    { MessageType.Voice, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendVoiceAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

    { MessageType.Sticker, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendStickerAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

    { MessageType.VideoNote, async (message, fileId, botClient, cancellationToken) =>
    await botClient.SendVideoNoteAsync(message.Chat.Id, fileId, replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },

};
    protected string? Text { get; set; }
    protected MessageType Type { get; set; }
    protected string? FileId { get; set; }

    #endregion

    public MessageHandler(Message message)
    {
        if (message.ReplyToMessage == null)
        {
            throw new TelegramExeption("You MUST reply to message", message);
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
            _ => throw new TelegramExeption("Unexpected message type", message),
        };
    }

    public virtual async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        ChatId chatId = message.Chat.Id;

        // Dictionary<MessageType, Func<Task>> messageHandlers = new Dictionary<MessageType, Func<Task>>
        // {
        // 	{ MessageType.Text, async () => await botClient.SendTextMessageAsync(chatId, Text, replyToMessageId: message.MessageId, parseMode: ParseMode.Html, cancellationToken: cancellationToken) },
        // 	{ MessageType.Photo, async () => await botClient.SendPhotoAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        // 	{ MessageType.Audio, async () => await botClient.SendAudioAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        // 	{ MessageType.Video, async () => await botClient.SendVideoAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        // 	{ MessageType.Voice, async () => await botClient.SendVoiceAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        // 	{ MessageType.Sticker, async () => await botClient.SendStickerAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        // 	{ MessageType.VideoNote, async () => await botClient.SendVideoNoteAsync(chatId, InputFile.FromFileId(FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        // };

        if (messageHandlers.ContainsKey(Type))
        {
            await messageHandlers[Type](message, InputFile.FromFileId(FileId), botClient, cancellationToken);
        }
        else
        {
            return;
        }
    }

}
