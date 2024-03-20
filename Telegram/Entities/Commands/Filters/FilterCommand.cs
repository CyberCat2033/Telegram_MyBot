using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands.Filters;

namespace Telegramchik.Commands;

public class FilterCommand 
{
    
    public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken, IFilter filter)
    {
        ChatId chatId = message.Chat.Id;

        Dictionary<MessageType, Func<Task>> messageHandlers = new Dictionary<MessageType, Func<Task>>
    {
        { MessageType.Text, async () => await botClient.SendTextMessageAsync(chatId, filter.Text, replyToMessageId: message.MessageId, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken) },
        { MessageType.Photo, async () => await botClient.SendPhotoAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        { MessageType.Audio, async () => await botClient.SendAudioAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        { MessageType.Video, async () => await botClient.SendVideoAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
        { MessageType.Voice, async () => await botClient.SendVoiceAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        { MessageType.Sticker, async () => await botClient.SendStickerAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        { MessageType.VideoNote, async () => await botClient.SendVideoNoteAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
    };

        if (messageHandlers.ContainsKey(message.Type))
        {
            await messageHandlers[message.Type].Invoke();
        }
        else
        {
            return;
        }
    }
}
