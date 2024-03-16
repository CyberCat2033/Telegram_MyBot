using System.Threading;

namespace Telegram;

public class Program
{
    async public static Task Main(string[] args)
    {
        CancellationTokenSource cts = new();
        var tg_bot = new Telegram_Botik(token: @"6095156109:AAEcsa618XaTXLToVcmLsk0WoumXfnICk3s",
            cts);
        await tg_bot.Start();
        Console.ReadLine();
        await tg_bot.Stop();
    }
}
