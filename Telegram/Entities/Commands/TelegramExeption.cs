using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class TelegramExeption : Exception
{
    public readonly Message TelegramMessage;

    public TelegramExeption(string message, Message TelegramMessage) : base(message)
    {
        this.TelegramMessage = TelegramMessage;
    }

}