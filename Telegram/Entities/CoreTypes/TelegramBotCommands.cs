using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands;

public abstract class TelegramBotCommands : BotCommand
{

	#region Consotructor
	public TelegramBotCommands(string Command, string Description = "")
	{
		this.Command = Command;
		this.Description = Description;
	}

	#endregion

	#region Abstract Methods
	public abstract Task ExecuteAsync(
		Message message, ITelegramBotClient botClient,
		 CancellationToken cancelationToken);

	#endregion

}
