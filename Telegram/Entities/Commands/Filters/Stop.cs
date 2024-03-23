using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik;

namespace Telegramchik.Commands.Filters;

public class Stop : TelegramCommands
{
    public Stop(string Command, string Description = "") : base(Command, Description)
    {
    }

    public override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        throw new NotImplementedException();
    }
}
