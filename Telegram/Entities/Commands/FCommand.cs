using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class FCommand : Telegram_Command
{
    List<string> Stickers_URLs = new();


    public FCommand(string Command, string Description = "") : base(Command, Description) { }

    public void AddStikerByURL(string StickerURL) => Stickers_URLs.Add(StickerURL);
    public void AddStikerByURL(string[] StickerURLs) => Stickers_URLs.Concat(StickerURLs);

    private InputFileUrl GetRandomSticker() => InputFile.FromUri(Stickers_URLs[new Random().Next(0, Stickers_URLs.Count() - 1)]);


    public override async  Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        await botClient.SendStickerAsync(
            chatId: message.Chat.Id,
            sticker: GetRandomSticker(),
            cancellationToken: CT
            );
    }
}
