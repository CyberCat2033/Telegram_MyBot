using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace Telegram;

public class Telegram_Botik
{
    public string StartTime { get; init; }
    public string Name { get; init;}
    ReceiverOptions receiverOptions;
    public CancellationTokenSource cts;

    Task<Bot.Types.User> me;

    TelegramBotClient botClient;

    public Telegram_Botik(string token)
    {
        botClient = new TelegramBotClient(token);
        StartTime = DateTime.Now.ToString();
        me =  botClient.GetMeAsync();
        

        cts = new();

        receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

    }
    async public Task Start()
    {
        botClient.StartReceiving
            (
     updateHandler: HandleUpdateAsync,
     pollingErrorHandler: HandlePollingErrorAsync,
     receiverOptions: receiverOptions,
     cancellationToken: cts.Token
            );


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

        // Echo received message text
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: token);
    }
}
