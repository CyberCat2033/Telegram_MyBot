#region usings
using Telegramchik.Commands;
using System.Diagnostics;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
#endregion

namespace Telegramchik;

public class Telegram_Botik
{
    #region Properties and fields 
    public string StartTime { get; init; }
    public string? Name { get; init; }
    public CancellationTokenSource cts;
    //public User me { get; init; }
    private IEnumerable<BotCommand> MyCommands;
    public string StopTime { get; private set; }
    private ReceiverOptions receiverOptions { get; init; }
    private ITelegramBotClient botClient;
    private FCommand FCommand;
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
        

        MyCommands =
        [

            new BotCommand{Command="/start",Description="Start Bot"},
            new BotCommand{Command="/filter",Description="Add Filter"},
            new BotCommand{Command="/stop",Description="Stop Bot"},
            FCommand = new("/f", "Press f")
        ];

        

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
        await botClient.SetMyCommandsAsync(MyCommands);
    }

    public async Task Test()
    {

        BotCommand[] currentCommands = await botClient.GetMyCommandsAsync();
        foreach ( BotCommand command in currentCommands )
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
        var chatId = message.Chat.Id;

        if (message.Type == MessageType.Text && messageText.ToLower()[0] == '/')
        {
            if (messageText.ToLower() == FCommand.Command)
            {
                await FCommand.Execute(message, client, token);
            }
        }

    }
    #endregion


}
