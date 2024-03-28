using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands.Filters;

public static class FiltersGroup
{
	private static ConcurrentDictionary<long, FilterCollection> FiltersDictionary { get; } = new();

	public static async Task Add(Message message)
	{
		long ChatId = message.Chat.Id;
		FiltersDictionary.TryAdd(ChatId, new FilterCollection());
		await FiltersDictionary[ChatId].Add(message);
	}

	public static async Task Remove(Message message)
	{
		if (message.Text.Split().Length < 2)
		{
			throw new Exception("Stop Command must contains keyword");
		}
		long ChatId = message.Chat.Id;
		await FiltersDictionary[ChatId].Remove(message);
	}

	public static bool TryGetValue(long key, out FilterCollection flc)
	{
		return FiltersDictionary.TryGetValue(key, out flc);
	}


}
