using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegramchik.Commands.Filters;

public class Filters
{
    public Dictionary<string, Filter> Filters_Dict = new();

    public async Task Add(Message message) => await Add(new Filter(message));

    public async Task Add(Filter filter)
    {
        await Task.Run(() =>
        {
            if (Filters_Dict.TryAdd(filter.Name, filter)) return;
            Filters_Dict[filter.Name] = filter;
        });
    }
}
