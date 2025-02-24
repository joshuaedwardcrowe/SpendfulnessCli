using YnabCli.Database.Users;

namespace YnabCli.Database.Settings;

public class Setting
{
    public int Id { get; set; }
    public string Value { get; set; }
    public SettingType Type { get; set; }
    public User User { get; set; }
}