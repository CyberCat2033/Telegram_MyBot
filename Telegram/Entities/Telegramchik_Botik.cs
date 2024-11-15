#region usings
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands;
using Telegramchik.Commands.Filters;
using Telegramchik.Commands.Goodbye;
using Telegramchik.Greeting;
using Telegramchik.SettingsManagment;

#endregion

namespace Telegramchik;

public class Telegramchik_Botik
{
    #region Properties and Fields
    public string StartTime { get; init; }
    public CancellationTokenSource cts;
    public string? StopTime { get; private set; }
    private ReceiverOptions receiverOptions { get; init; }
    private readonly ITelegramBotClient botClient;
    private readonly Dictionary<string, TelegramBotCommands> CommandDict;

    #endregion

    #region Constructor
    public Telegramchik_Botik(string token, CancellationTokenSource CTSource)
    {
        botClient = new TelegramBotClient(token);
        StartTime = DateTime.Now.ToString();
        cts = CTSource;
        receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        CommandDict = new()
        {
            ["/f"] = new FCommand("/f", "Press F"),
            ["/filter"] = new FilterCommand("/filter", "Add Filter"),
            ["/stop"] = new StopCommand("/stop", "Stop Filter"),
            ["/start"] = new StartCommand("/start", "Start bot"),
            ["/setwelcome"] = new SetWelcomeCommand("/setwelcome", "Change welcome command"),
            ["/setgoodbye"] = new SetGoodbyeCommand("/setgoodbye", "Change goodbye command"),
        };
    }
    #endregion


    #region Public Methods
    public async Task Start()
    {
        await start_notification();
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
        await botClient.SetMyCommandsAsync(CommandDict.Select(x => x.Value));
    }

    public async Task Stop()
    {
        StopTime = DateTime.Now.ToString();
        cts.Cancel();
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Out.WriteAsync($"The Bot was stopped at {StopTime}");
        Console.ResetColor();
    }
    #endregion

    #region Private Methods
    private async Task start_notification()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        await Console.Out.WriteAsync($"Start listening at {StartTime}");
        Console.ResetColor();
        await Console.Out.WriteLineAsync();
    }

    private async Task HandlePollingErrorAsync(
        ITelegramBotClient client,
        Exception exception,
        CancellationToken token
    )
    {
        Console.WriteLine(exception.Message);
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient client,
        Update update,
        CancellationToken token
    )
    {
        var message = update.Message;
        var chatId = message.Chat.Id;
        if (!SettingsFactory.Contains(chatId) && message.Text != "/start")
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Pleease start bot before using it",
                replyToMessageId: message.MessageId,
                cancellationToken: token
            );
            return;
        }
        if (Conditions.WelcomeCondition(update))
        {
            try
            {
                var settings = SettingsFactory.TryGet(chatId);
                await settings.GetWelcomeMessage().ExecuteAsync(message, botClient, token);
            }
            catch (Exception exc)
            {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: exc.Message,
                    replyToMessageId: update.Message.MessageId,
                    cancellationToken: token
                );
                return;
            }
        }
        if (Conditions.GoodbyeCondition(update))
        {
            try
            {
                var settings = SettingsFactory.TryGet(update.Message.Chat.Id);
                await settings.GetGoodbyeMessage().ExecuteAsync(update.Message, botClient, token);
            }
            catch (Exception exc)
            {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: exc.Message,
                    replyToMessageId: update.Message.MessageId,
                    cancellationToken: token
                );
            }
            return;
        }

        if (message is not { })
            return;
        if (message.Text is not { } messageText)
            return;

        if (message.Type == MessageType.Text && messageText.ToLower()[0] == '/')
        {
            if (CommandDict.TryGetValue(messageText.ToLower().Split()[0], out var telegramCommands))
            {
                try
                {
                    await telegramCommands.ExecuteAsync(message, client, token);
                }
                catch (Exception exc)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: exc.Message,
                        replyToMessageId: message.MessageId,
                        cancellationToken: token
                    );
                }
            }
        }
        else
        {
            try
            {
                await MessageParser.Parse(message, botClient, token);
            }
            catch (Exception exc)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: exc.Message,
                    replyToMessageId: message.MessageId,
                    cancellationToken: token
                );
            }
            try
            {
                if (message.Type == MessageType.Text && messageText.ToLower()[0] == '/')
                {
                    if (
                        CommandDict.TryGetValue(
                            messageText.ToLower().Split()[0],
                            out var telegramCommands
                        )
                    )
                    {
                        await telegramCommands.ExecuteAsync(message, client, token);
                    }
                }
                else
                {
                    await MessageParser.Parse(message, botClient, token);
                }
            }
            catch (Exception exc)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: exc.Message,
                    replyToMessageId: message.MessageId,
                    cancellationToken: token
                );
            }
        }
    }

    #endregion
}
