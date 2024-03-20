using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegramchik.Commands;

public class FCommand : TelegramCommands
{
    private List<string> Stciker_IDs = new() {
    "CAACAgIAAxkBAAICMmX62PS9RFDMHb2On7G9DOjKRbnWAAIMAQACTptkAmOSrBs0ItNHNAQ",
    "CAACAgIAAxkBAAICNWX62QEWGPMfuMwT5vrKEEHnAfXpAAIdAQACTptkAnofe0zzYUy2NAQ",
    "CAACAgIAAxkBAAICOGX62Q4Pqga0wRQJQ8318dMix5z0AAJJAQACTptkAmgp7D2NPAz-NAQ",
    "CAACAgIAAxkBAAICO2X62SU7RssdPTUVSMllmI8_jv5UAAK7AANOm2QCTTNNKc65rHw0BA",
    "CAACAgIAAxkBAAICPmX62TyzZAST_iSxB_40KgXAynKzAALHFQACOk4JSQFAjkLpBTNnNAQ",
    };

    public FCommand(string Command, string Description = "") : base(Command, Description) { }

    private InputFileId GetRandomSticker() => InputFile.FromFileId(Stciker_IDs[new Random().Next(0, Stciker_IDs.Count() - 1)]);

    public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CancelationToken)
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
