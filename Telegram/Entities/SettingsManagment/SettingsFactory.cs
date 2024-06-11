using System.Collections.Concurrent;
using Telegramchik.Commands;

namespace Telegramchik.SettingsManagment;

public static class SettingsFactory
{
    private static ConcurrentDictionary<long, Settings> _settings = new();

    public static bool TryAdd(long ChatId) => _settings.TryAdd(ChatId, new Settings());

    public static Settings TryGet(long ChatId)
    {
        return _settings.TryGetValue(ChatId, out var settings)
            ? settings
            : throw new TelegramExeption("Pleease start bot before using it");
    }

    public static bool Contains(long ChatId) => _settings.ContainsKey(ChatId);
}
