using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegramchik.Commands.Filters;

namespace Telegramchik.Settings;

public class Settings
{
    private ConcurrentDictionary<string, Filter> Filters_Dict = new();

    #region Filters

    public async Task AddFilter(Message message)
    {
        Filter filter= new(message);
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

    public bool TryGetFilter(string key, out Filter fl)
    {
        return Filters_Dict.TryGetValue(key, out fl);

    }
    #endregion

    #region Greating

    #endregion
}
