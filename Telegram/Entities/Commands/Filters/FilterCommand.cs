using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands.Filters;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Commands;

public class FilterCommand : TelegramBotCommands
{
	public FilterCommand(string Command, string Description = "") : base(Command, Description)
	{
	}

	public async override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken cancelationToken)
	{
		var settings = SettingsFactory.TryGet(message.Chat.Id);
		await settings.AddFilter(message).ConfigureAwait(false);
		await MessageSenderAndDeleter.DoMess(message,
		botClient,
		cancelationToken,
		text: $"The filter \"{message.Text.Split()[1]}\" has been added");
	}
}

