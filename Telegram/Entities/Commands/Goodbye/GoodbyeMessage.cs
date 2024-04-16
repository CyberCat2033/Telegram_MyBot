using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegramchik.Commands.Goodbye;

public class GoodbyeMessage : MessageHandler
{
    public GoodbyeMessage(Message message) : base(message)
    {
        base.ParseMessage(message);
    }
    public GoodbyeMessage()
    {
        Type = MessageType.Text;
        Text = "A member has left our chat. Let's wish him well " +
            "on his future endeavors. We'll miss their contributions and " +
            "hope they keep in touch!";
        FileId = null;
    }
}
