using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik.CoreTypes;


namespace Telegramchik.Greeting;

public class WelcomeMessage : MessageHandler
{
	public WelcomeMessage(Message message) : base(message)
	{
		base.ParseMessage(message);
	}


	public WelcomeMessage()
	{
		Type = MessageType.Text;
		Text = "Hello and welcome to our Telegram community! " +
			"I'm your friendly bot, here to assist you. " +
			"If you have any questions or need help getting started, just type '/help'" +
			" and I'll be right there! Enjoy your time here and don't hesitate to reach" +
			" out. 😊";
		FileId = null;
	}
}
