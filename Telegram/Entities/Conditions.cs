using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace Telegramchik;

public static class Conditions
{
	public static bool WelcomeCondition(Update update) => update.Message.NewChatMembers != null;
	public static bool GoodbyeCondition(Update update) => update.Message.LeftChatMember != null;
}