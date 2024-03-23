﻿#region usings
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands;
using Telegramchik.Commands.Filters;
#endregion

namespace Telegramchik;

public class Telegramchik_Botik
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
    public Telegramchik_Botik(string token, CancellationTokenSource CTSource)
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
            ["/filter"] = new FilterCommand("/filter", "Add Filter")

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

        await ExeptionHandler.ChangeFields(message ,client, token);

        if (message.Type == MessageType.Text && messageText.ToLower()[0] == '/')
        {
            
            TelegramCommands telegramCommands;
            if (CommandDict.TryGetValue(messageText.ToLower().Split()[0], out telegramCommands))
            {
                try
                {
                    await telegramCommands.ExecuteAsync(message, client, token);
                }
                catch (ArgumentException exc)
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
            await StringFilterParser(message, botClient, token);

        }
        #endregion


    }

    public async Task StringFilterParser(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (message.Type != MessageType.Text) return;
        var array = new[] { ".", ",", "/", "?", "!", "@", "#", "$", "*", "^", "(", ")" };
        if (!FiltersGroup.TryGetValue(message.Chat.Id, out var filterCollection)) return;
        foreach (var mes in message.Text.ToLower().Split())
        {
            if (filterCollection.TryGetValue(mes, out var fl))
            {
                await ExecuteAsync(message, botClient, cancellationToken, fl);
            }
        }
    }

    public async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken, Filter filter)
    {
        ChatId chatId = message.Chat.Id;

        Dictionary<MessageType, Func<Task>> messageHandlers = new Dictionary<MessageType, Func<Task>>
        {
            { MessageType.Text, async () => await botClient.SendTextMessageAsync(chatId, filter.Text, replyToMessageId: message.MessageId, parseMode: ParseMode.MarkdownV2, cancellationToken: cancellationToken) },
            { MessageType.Photo, async () => await botClient.SendPhotoAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Audio, async () => await botClient.SendAudioAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Video, async () => await botClient.SendVideoAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken: cancellationToken) },
            { MessageType.Voice, async () => await botClient.SendVoiceAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.Sticker, async () => await botClient.SendStickerAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
            { MessageType.VideoNote, async () => await botClient.SendVideoNoteAsync(chatId, InputFile.FromFileId(filter.FileId), replyToMessageId: message.MessageId, cancellationToken : cancellationToken) },
        };

        if (messageHandlers.ContainsKey(filter.Type))
        {
            await messageHandlers[filter.Type].Invoke();
        }
        else
        {
            return;
        }
    }
}