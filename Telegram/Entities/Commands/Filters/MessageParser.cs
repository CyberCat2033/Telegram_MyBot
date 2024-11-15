using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.SettingsManagment;

namespace Telegramchik.Commands.Filters;

public static class MessageParser
{
	#region Properties and Fields 
	private static readonly char[] removeChars = ['.', ',', '/', '?', '!', '@', '#', '$', '*', '^', '(', ')'];
	
	#endregion
	
	#region Methods
	public static async Task Parse(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
	{
		if (message is not { Type: MessageType.Text, Text: not null }) return;
		var filterCollection = SettingsManagment.SettingsFactory.TryGet(message.Chat.Id);
		var filterText = message.Text.ToLower().Split().Select(x => x.Trim(removeChars));
		foreach (var mes in filterText)
		{
			if (filterCollection.TryGetFilter(mes, out var filter))
			{
				await filter.ExecuteAsync(message, botClient, cancellationToken);
				return;
			}
		}
	}
	
	#endregion
}