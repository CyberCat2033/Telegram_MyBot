using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram;

public class Telegram_Botik
{
    #region Properties and fields 
    public string StartTime { get; init; }
    public string? Name { get; init; }
    public CancellationTokenSource cts;
    public User me { get; init; }

    private ReceiverOptions receiverOptions { get; init; }
    private TelegramBotClient botClient;
    #endregion

    #region Constructor
    public Telegram_Botik(string token)
    {
        botClient = new TelegramBotClient(token);
        StartTime = DateTime.Now.ToString();
        me = botClient.GetMeAsync().Result;
        Name = me.Username;
        

        cts = new();

        receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() 
        };

    }
    #endregion

    #region Private Methods
    private void start_notification()
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Start listening in {StartTime}");
        Console.ResetColor();
    }

    private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;
        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: token);
    }
    #endregion

    #region Public Methods
    public async Task Start()
    {
        start_notification();
        botClient.StartReceiving
            (
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
            );
    }
    #endregion

}
