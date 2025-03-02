using System.ComponentModel.DataAnnotations;
using YnabCli.Database.Users;

namespace YnabCli.Database.Settings;

public class Setting
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public  required string Value { get; set; }
    public  required SettingType Type { get; set; }
    public  required User User { get; set; }
}