using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.Commands.Filters;
using Telegramchik.Settings;

namespace Telegramchik.Commands;

public class FilterCommand : TelegramCommands
{
	public FilterCommand(string Command, string Description = "") : base(Command, Description)
	{
	}

	public async override Task ExecuteAsync(Message message, ITelegramBotClient botClient, CancellationToken CT)
	{
		var settings = SettingsFactory.TryGet(message.Chat.Id);
		await settings.AddFilter(message).ConfigureAwait(false);
		await MessageSenderAndDeleter.SendMessageAndDeleteAsync(message,
		botClient, CT,
		text: $"The filter \"{message.Text.Split()[1]}\" has been added");
	}
}

