#define DEBUG
#undef DEBUG
#region using
using System.Diagnostics;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
#endregion

namespace Telegram;

public class Telegram_Botik
{
    #region Properties and fields 
    public string StartTime { get; init; }
    public string? Name { get; init; }
    public CancellationTokenSource cts;
    public User me { get; init; }
    private IEnumerable<BotCommand> commandsToBeSet;
    public string StopTime { get; private set; }
    private ReceiverOptions receiverOptions { get; init; }
    private TelegramBotClient botClient;
    #endregion

    #region Constructor
    public Telegram_Botik(string token, CancellationTokenSource CTSource)
    {
        botClient = new TelegramBotClient(token);
        StartTime = DateTime.Now.ToString();
        me = botClient.GetMeAsync().Result;
        Name = me.Username;
        cts = CTSource;

        receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        commandsToBeSet =
        [

            new BotCommand{Command="/start",Description="Start Bot"},
            new BotCommand{Command="/filter",Description="Add Filter"},
            new BotCommand{Command="/stop",Description="Stop Bot"},
            new BotCommand{Command="/f",Description="Press f"},
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
        await botClient.SetMyCommandsAsync(commandsToBeSet);
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
        throw new NotImplementedException();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {

        #if DEBUG
            var sw = new Stopwatch();
            sw.Start();
        #endif

        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;
        var chatId = message.Chat.Id;

        #if DEBUG
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText + $" \nTime elapsed: {sw.ElapsedMilliseconds} ms",
                replyMarkup: new ReplyKeyboardRemove(),
                replyToMessageId: update.Message.MessageId,
                cancellationToken: token);
        #else
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                replyMarkup: new ReplyKeyboardRemove(),
                replyToMessageId: update.Message.MessageId,
                cancellationToken: token);

        #endif


    }
    #endregion


}
