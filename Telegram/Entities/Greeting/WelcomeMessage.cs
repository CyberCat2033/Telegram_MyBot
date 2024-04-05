using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegramchik;

namespace Telegramchiik.Greeting;

public class WelcomeMessage : IMessageProperties
{
    public string? Text { get; private set; }
    public MessageType Type { get; private set; }
    public string? FileId { get; private set; }
}
