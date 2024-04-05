using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik;

namespace Telegramchiik.Greeting;

public class WelcomeMessage : MessageHandler
{
    public WelcomeMessage(Message message) : base(message)
    {
        ParseMessage(message);
    }

    private void ParseMessage(Message message)
    {
        base.ParseMessage(message);
    }
}
