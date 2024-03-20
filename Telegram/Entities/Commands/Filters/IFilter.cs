namespace Telegramchik.Commands.Filters;
using Telegram.Bot.Types.Enums;

public interface IFilter
{
    public string Text { get; set; }
    public MessageType Type { get; set; }
    public string FileId { get; set; }
    public string Name { get; set; }

}
