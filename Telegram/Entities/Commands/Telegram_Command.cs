using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public abstract class Telegram_Command
{
    public string Command { get; init; }
    public string? Description { get; private set; }

    public  Telegram_Command(string Command, string Description = "") 
    {
        this.Command = Command;
        this.Description = Description;
    }

    public abstract Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CT);

}
