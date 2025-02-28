namespace YnabCli.Database.Settings;

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
}