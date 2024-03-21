using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegramchik.Commands.Filters
{
    public static class FiltersGroup
    {
        private static Dictionary<long, FilterCollection> FiltersDictionary { get; } = new();

        public static async Task Add( Message message)
        {
            long ChatId = message.Chat.Id;
            await Task.Run(() =>
            {
                if (!FiltersDictionary.TryAdd(ChatId, new FilterCollection()))
                {
                    FiltersDictionary[ChatId] = new FilterCollection();
                }
                FiltersDictionary[ChatId].Add(message);
            });
        }
        public static bool TryGetValue(long key, out FilterCollection flc)
        {
            return FiltersDictionary.TryGetValue(key, out flc);
        }


    }
}
