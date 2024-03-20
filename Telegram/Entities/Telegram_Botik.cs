#region usings
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands;
#endregion

namespace Telegramchik;

public class Telegram_Botik
{
    #region Properties and fields 
    public string StartTime { get; init; }
    public string? Name { get; init; }
    public CancellationTokenSource cts;
    public string StopTime { get; private set; }
    private ReceiverOptions receiverOptions { get; init; }
    private ITelegramBotClient botClient;
    private Dictionary<string, TelegramCommands> CommandDict;

    #endregion

    #region Constructor
    public Telegram_Botik(string token, CancellationTokenSource CTSource)
    {
        botClient = new TelegramBotClient(token);
        StartTime = DateTime.Now.ToString();
        //me = botClient.GetMeAsync().Result;
        //Name = me.Username;
        cts = CTSource;
        receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        CommandDict = new()
        {
            ["/f"] = new FCommand("/f", "Press F"),

        };






    }
    #endregion

    #region Public Methods
    public async Task Start()
    {
        await start_notification();
        botClient.StartReceiving
            (
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
            );
        await botClient.SetMyCommandsAsync(CommandDict.Select(x => x.Value));
    }

    public async Task Test()
    {

        BotCommand[] currentCommands = await botClient.GetMyCommandsAsync();
        foreach (BotCommand command in currentCommands)
        {
            await Console.Out.WriteLineAsync(command.Command);
        }

    }

    public async Task Stop()
    {
        StopTime = DateTime.Now.ToString();
        cts.Cancel();
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Out.WriteLineAsync($"The Bot was stopped at {StopTime}");
        Console.ResetColor();
    }
    #endregion

    #region Private Methods
    private async Task start_notification()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Out.WriteLineAsync($"Start listening at {StartTime}");
        Console.ResetColor();
    }


    private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        await Console.Out.WriteLineAsync(exception.Message);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;
        long chatId = message.Chat.Id;

        if (message.Type == MessageType.Text && messageText.ToLower()[0] == '/')
        {
            TelegramCommands telegramCommands;
            await Task.Run(() =>
            {
                if (CommandDict.TryGetValue(messageText.ToLower(), out telegramCommands))
                {
                    telegramCommands.ExecuteAsync(message, client, token);
                }

            });
            
            

        }
        #endregion


    }
}
