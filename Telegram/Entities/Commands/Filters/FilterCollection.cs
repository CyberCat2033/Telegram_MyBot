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
    private Dictionary<string, Filter> Filters_Dict = new();

    public async Task Add(Message message) 
        => await Add(new Filter(message));

    public async Task Add(Filter filter)
    {
        await Task.Run(() =>
        {
            Filters_Dict[filter.Name] = filter;
        });
    }

    public async Task Remove(Message message)
    {
        await Task.Run(() =>
        {
            Filters_Dict.Remove(message.Text.Split()[1]);
        });

    }

    public bool TryGetValue(string key, out Filter fl)
    {
        return Filters_Dict.TryGetValue(key, out fl); 
        
    }
}
