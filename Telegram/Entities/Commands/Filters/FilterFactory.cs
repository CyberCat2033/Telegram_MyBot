using System.Collections.Concurrent;
using Telegram.Bot.Types;

namespace Telegramchik.Commands.Filters;

public class FilterFactory
{
    private ConcurrentDictionary<string, Filter> Filters_Dict = new();

    public async Task Add(Message message)
    {
        Filter filter = new(message);
        await Task.Run(() =>
        {
            Filters_Dict[filter.Name.ToLower()] = filter;
        });
    }

    public async Task TryRemove(Message message)
    {
        Filter val;
        if (!Filters_Dict.TryRemove(message.Text.Split()[1], out val))
        {
            throw new TelegramExeption("This chat doesn`t contains such a filter");
        }

    }

    public bool TryGetValue(string key, out Filter fl)
    {
        return Filters_Dict.TryGetValue(key, out fl);

    }
}
