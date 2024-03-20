namespace Telegramchik.Commands.Filters;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

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
        string separator = "";
        byte index = 1; 
        if (message.Text.Contains("\"")) 
        { 
            separator = "\""; 
            index = 0;
        }
        FileId = GetFileId(message);
        Name = message.Text.Split(separator)[index];
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
