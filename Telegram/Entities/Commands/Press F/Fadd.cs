using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class Fadd : TelegramCommands
{
    public Fadd(string Command, string Description = "") : base(Command, Description)
    {
    }

    public async override Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        throw new NotImplementedException();
    }
}
