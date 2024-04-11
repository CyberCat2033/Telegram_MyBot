using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Telegramchik.Commands;

public static class MessageSenderAndDeleter
{
	public static async Task DoMess(
		Message message,
		ITelegramBotClient botClient,
		CancellationToken cancelationToken,
		string text,
		bool DeleteSelfMessage = true
	)
	{
		await botClient.SendTextMessageAsync(
			chatId: message.Chat.Id,
			text: text,
			cancellationToken: cancelationToken
		);
		await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, cancelationToken);
		await Task.Delay(1000);
		if (DeleteSelfMessage) await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId + 1, cancelationToken);
	}
}
