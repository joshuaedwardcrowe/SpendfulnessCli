using Spendfulness.Database.Settings;

namespace SpendfulnessCli.Commands.Extensions;

public static class SettingsExtensions
{
    public static string? FirstX(this ICollection<Setting> settings, string settingType)
    {
        var setting = settings.FirstOrDefault(y => y.Type.Name == settingType);
        return setting?.Value;
    }
}