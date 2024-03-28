namespace Telegramchik;

public class Program
{
    async public static Task Main(string[] args)
    {
        CancellationTokenSource cts = new();
        Telegramchik_Botik tg_bot = new Telegramchik_Botik(token: @"6095156109:AAEcsa618XaTXLToVcmLsk0WoumXfnICk3s",
            CTSource: cts);
        await tg_bot.Start();
        //tg_bot.Test();
        while (true)
        {
            if (Console.ReadLine()?.ToLower() is not "/stop" and not "/exit")
            {
                continue;
            }
            await tg_bot.Stop();
            break;
        }
        Console.Read();
    }
}
