using System.Threading;
using System;
using System.Threading.Channels;

namespace Telegramchik;

public class Program
{
    async public static Task Main(string[] args)
    {
        CancellationTokenSource cts = new();
        var tg_bot = new Telegram_Botik(token: @"6095156109:AAEcsa618XaTXLToVcmLsk0WoumXfnICk3s",
            CTSource: cts);
        await tg_bot.Start();
        tg_bot.Test();
        while (true)
        {
            if (Console.ReadLine().ToLower() is "/stop" or "/exit")
            {
                await tg_bot.Stop();
                break;
            }
        }
        Console.Read();
    }
}
