namespace Telegramchik.Commands.Filters;

using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik;

public class Filter : MessageHandler
{
    public string Name { get; private set; }

    public Filter(Message message) : base(message)
    {
        ParseMessage(message);
    }

    private void ParseMessage(Message message)
    {
        base.ParseMessage(message);
        Name = message.Text.Split()[1];
    }

}
