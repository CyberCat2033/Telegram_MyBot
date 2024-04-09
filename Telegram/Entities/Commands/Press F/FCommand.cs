using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class FCommand : TelegramBotCommands
{
    private readonly string[] Stcikers_IDs = {
    "CAACAgIAAxkBAAICMmX62PS9RFDMHb2On7G9DOjKRbnWAAIMAQACTptkAmOSrBs0ItNHNAQ",
    "CAACAgIAAxkBAAICNWX62QEWGPMfuMwT5vrKEEHnAfXpAAIdAQACTptkAnofe0zzYUy2NAQ",
    "CAACAgIAAxkBAAICOGX62Q4Pqga0wRQJQ8318dMix5z0AAJJAQACTptkAmgp7D2NPAz-NAQ",
    "CAACAgIAAxkBAAICO2X62SU7RssdPTUVSMllmI8_jv5UAAK7AANOm2QCTTNNKc65rHw0BA",
    "CAACAgIAAxkBAAICPmX62TyzZAST_iSxB_40KgXAynKzAALHFQACOk4JSQFAjkLpBTNnNAQ",
    };

    public FCommand(string Command, string Description) : base(Command, Description) { }

    private InputFileId GetRandomSticker()
    {
        
        return InputFile.FromFileId(Stcikers_IDs[new Random().Next(0, Stcikers_IDs.Length)]);
    }

    public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CancellationToken)
    {
        long ChatId = message.Chat.Id;

        await botClient.SendStickerAsync(
            chatId: ChatId,
            sticker: GetRandomSticker(),
            cancellationToken: CancellationToken
            );

        await botClient.DeleteMessageAsync(
            chatId: ChatId,
            messageId: message.MessageId,
            cancellationToken: CancellationToken
            );


    }
}
