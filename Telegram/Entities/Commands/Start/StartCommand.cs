using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public class StartCommand : TelegramCommands
{
	public StartCommand(string Command, string Description = "") : base(Command, Description)
	{
	}

	public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
	{
		string GreatingText = $"Hi, my name is {botClient.GetMeAsync().Result.FirstName}. I`m a very stupid, " +
		"but interesting bot";
		long chatID = message.Chat.Id;
		await botClient.SendTextMessageAsync(
			chatID,
			GreatingText,
			cancellationToken: CT
		).ConfigureAwait(false);
		await Task.Delay(100);
		await botClient.DeleteMessageAsync(chatID, message.MessageId, CT).ConfigureAwait(false);
	}
}
