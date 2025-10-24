namespace Cli.Spendfulness.Database.Settings;

public static class SettingsExtensions
{
    public static string? AsString(this ICollection<Setting> settings, string settingType)
    {
        var setting = settings.FirstOrDefault(y => y.Type.Name == settingType);
        return setting?.Value;
    }

    public static Guid? AsGuid(this ICollection<Setting> settings, string settingType)
    {
        var setting = settings.FirstOrDefault(y => y.Type.Name == settingType);

        if (!Guid.TryParse(setting?.Value, out var result))
        {
            return null;
        }

        return result;
    }

    public static int? AsInt(this ICollection<Setting> settings, string settingType)
    {
        var setting = settings.FirstOrDefault(y => y.Type.Name == settingType);

        if (!int.TryParse(setting?.Value, out var result))
        {
            return null;
        }

        return result;
    }
}