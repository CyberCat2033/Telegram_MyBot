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

    public async Task Add(Message message, ITelegramBotClient botClient) 
        => await Add(new Filter(message,botClient));

    public async Task Add(Filter filter)
    {
        await Task.Run(() =>
        {
            Filters_Dict[filter.Name] = filter;
        });
    }

    public bool TryGetValue(string key, out Filter fl)
    {
        return Filters_Dict.TryGetValue(key, out fl); 
        
    }
}
