using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Commands;

public class StartCommand : TelegramBotCommands
{
	public StartCommand(string Command, string Description = "") : base(Command, Description)
	{
	}

	public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
	{
		string GreatingText;
		if (SettingsFactory.TryAdd(message.Chat.Id))
		{
			GreatingText = $"Hi, my name is {botClient.GetMeAsync().Result.FirstName}. I`m a very stupid, " +
		"but interesting bot";
		}
		else GreatingText = "The bot is already running";

		await MessageSenderAndDeleter.SendMessageAndDeleteAsync(message,
		botClient, CT,
		text: GreatingText,
		false);
	}
}
