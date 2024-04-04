using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegramchik.Settings;

public static class SettingsFactory
{
    private static ConcurrentDictionary<long, Settings> _settings = new();

    public static void TryAdd(long ChatId)
    {
        _settings.TryAdd(ChatId, new Settings());
    }

    public static Settings Get(long ChatId)
    {
        if(_settings.TryGetValue(ChatId, out var settings))
        {
            return settings;
        }
        throw new ArgumentException("There`s no settings with such ID");
    }
}
