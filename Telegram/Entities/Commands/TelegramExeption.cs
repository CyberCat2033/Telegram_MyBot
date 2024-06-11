using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class TelegramExeption : Exception
{

    public TelegramExeption(string message) : base(message)
    {
    }

}
