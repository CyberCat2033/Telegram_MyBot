using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegramchik.Commands.Filters;

public class FilterCollection
{
	private ConcurrentDictionary<string, Filter> Filters_Dict = new();

	public async Task Add(Message message)
	{
		Filter filter = new(message);
		await Task.Run(() =>
		{
			Filters_Dict[filter.Name] = filter;
		});
	}

	public async Task Remove(Message message)
	{
		Filter val;
		if (!Filters_Dict.TryRemove(message.Text.Split()[1], out val))
		{
			throw new ArgumentException("Filters doesn`t contains such a keyword");
		}

	}

	public bool TryGetValue(string key, out Filter fl)
	{
		return Filters_Dict.TryGetValue(key, out fl);

	}
}
