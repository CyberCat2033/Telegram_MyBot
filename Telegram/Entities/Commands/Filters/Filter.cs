namespace Telegramchik.Commands.Filters;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class Filter : IFilter
{
    public string? Text { get; set; }
    public MessageType Type { get; set; }
    public string? FileId { get; set; }
    public string Name { get; set; }

    public Filter(Message message)
    {
        if (message.ReplyToMessage == null)
            throw new Exception("No Reply");

        ParseMessage(message);
    }

    private void ParseMessage(Message message)
    {
        FileId = GetFileId(message.ReplyToMessage);
        if (message.Text.Split().Count() < 2)
        {
            throw new Exception("Filter Command must contains key_word");
        }
        Name = message.Text.Split()[1];
        Type = message.ReplyToMessage.Type;
        Text = message.ReplyToMessage.Type == MessageType.Text ? message.ReplyToMessage.Text : "";
    }

    private string? GetFileId(Message message)
    {
        return message.Type switch
        {
            MessageType.Sticker => message.Sticker.FileId,
            MessageType.Text => null,
            MessageType.Audio => message.Audio.FileId,
            MessageType.Video => message.Video.FileId,
            MessageType.Voice => message.Voice.FileId,
            MessageType.VideoNote => message.VideoNote.FileId,
            _ => throw new ArgumentException("Unexpected message type", nameof(message)),
        };
    }


}
