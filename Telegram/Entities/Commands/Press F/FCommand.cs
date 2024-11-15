using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class FCommand : TelegramBotCommands
{
	private int indexer;
	private readonly string[] Stcikers_IDs = {
	"CAACAgIAAxkBAAICMmX62PS9RFDMHb2On7G9DOjKRbnWAAIMAQACTptkAmOSrBs0ItNHNAQ",
	"CAACAgIAAxkBAAICNWX62QEWGPMfuMwT5vrKEEHnAfXpAAIdAQACTptkAnofe0zzYUy2NAQ",
	"CAACAgIAAxkBAAICOGX62Q4Pqga0wRQJQ8318dMix5z0AAJJAQACTptkAmgp7D2NPAz-NAQ",
	"CAACAgIAAxkBAAICO2X62SU7RssdPTUVSMllmI8_jv5UAAK7AANOm2QCTTNNKc65rHw0BA",
	"CAACAgIAAxkBAAICPmX62TyzZAST_iSxB_40KgXAynKzAALHFQACOk4JSQFAjkLpBTNnNAQ",
	};

	public FCommand(string Command, string Description) : base(Command, Description) { }

	private InputFileId RandomSticker => InputFile.FromFileId(Stcikers_IDs[TelegramHelper.Get(Stcikers_IDs.Length)]);

	// private int Get()
	// {
	// 	var tm_indexer = new Random().Next(0, Stcikers_IDs.Length);
	// 	indexer = indexer == tm_indexer ? (tm_indexer + 1) % Stcikers_IDs.Length - 1 : tm_indexer;
	// 	return indexer;
	// }

	public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CancellationToken)
	{
		long ChatId = message.Chat.Id;

		await botClient.SendStickerAsync(
			chatId: ChatId,
			sticker: RandomSticker,
			cancellationToken: CancellationToken
			);
		await Task.Delay(100);

		await botClient.DeleteMessageAsync(
			chatId: ChatId,
			messageId: message.MessageId,
			cancellationToken: CancellationToken
			);


	}
}
