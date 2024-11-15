using Telegram.Bot.Types;
using Telegramchik.CoreTypes;

namespace Telegramchik.Commands.Filters;

public class Filter : MessageHandler
{
	public string Name { get; private set; }

	public Filter(Message message) : base(message)
	{
		ParseMessage(message);
	}

	private void ParseMessage(Message message)
	{
		if (message.Text.Split().Count() < 2)
		{
			throw new TelegramExeption("This command MUST contains keyword");
		}
		base.ParseMessage(message);
		Name = message.Text.Split()[1];
	}

}
