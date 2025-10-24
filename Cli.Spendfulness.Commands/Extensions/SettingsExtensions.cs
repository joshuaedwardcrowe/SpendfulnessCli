using Cli.Spendfulness.Database.Settings;

namespace Cli.Spendfulness.Commands.Extensions;

public static class SettingsExtensions
{
    public static string? FirstX(this ICollection<Setting> settings, string settingType)
    {
        var setting = settings.FirstOrDefault(y => y.Type.Name == settingType);
        return setting?.Value;
    }
}