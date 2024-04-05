﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegramchik.Commands.Filters;

namespace Telegramchik.SettingsManagment;

public class Settings
{
	private ConcurrentDictionary<string, Filter> Filters_Dict = new();
	private string WelcomeMesage;

	#region Filters

	public async Task AddFilter(Message message)
	{
		var filter = new Filter(message);
		await Task.Run(() =>
		{
			Filters_Dict[filter.Name] = filter;
		});
	}

	public async Task RemoveFilter(Message message)
	{
		await Task.Run(() =>
		{
			if (!Filters_Dict.TryRemove(message.Text.Split()[1], out var val))
			{
				throw new ArgumentException("Filters doesn`t contains such a keyword");
			}
		});
	}

	public bool TryGetFilter(string key, out Filter filter)
	{
		return Filters_Dict.TryGetValue(key, out filter);

	}
	#endregion

	#region Greating

	#endregion
}
