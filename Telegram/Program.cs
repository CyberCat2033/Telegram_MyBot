﻿namespace Telegram;

public class Program
{
    async public static Task Main(string[] args)
    {
        var tg_bot = new Telegram_Botik(@"6095156109:AAEcsa618XaTXLToVcmLsk0WoumXfnICk3s");
        await tg_bot.Start();
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Start listening in {tg_bot.StartTime}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor= ConsoleColor.Black;
        Console.ReadLine();

        // Send cancellation request to stop bot
        tg_bot.cts.Cancel();
    }
}
