using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegramchik.Commands;

public class FCommand : TelegramCommands
{
    List<string> Stciker_IDs = new();


    public FCommand(string Command, string Description = "") : base(Command, Description) { }

    public void AddStikerByFileId(string StickerURL) => Stciker_IDs.Add(StickerURL);
    public void AddStikerByFileId(string[] StickerURLs) => Stciker_IDs.Concat(StickerURLs);

    private InputFileId GetRandomSticker() => InputFile.FromFileId(Stciker_IDs[new Random().Next(0, Stciker_IDs.Count() - 1)]);

    public void AddStickerByReply(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        if (message.ReplyToMessage.Type != MessageType.Sticker)
        {
            return;
        }

        AddStikerByFileId(message.ReplyToMessage.Sticker.FileId);

        botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
        text: "Fuck",
            cancellationToken: CT
            );
    }

    public override async  Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CancelationToken)
    {
        long ChatId = message.Chat.Id;

        await botClient.SendStickerAsync(
            chatId: ChatId,
            sticker: GetRandomSticker(),
            cancellationToken: CancelationToken
            );

        

        await botClient.DeleteMessageAsync(
            chatId: ChatId,
            messageId: message.MessageId,
            cancellationToken: CancelationToken
            );


    }
}
