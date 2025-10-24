using System.ComponentModel.DataAnnotations;
using Cli.Spendfulness.Database.Users;

namespace Cli.Spendfulness.Database.Settings;

public class Setting
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public  required string Value { get; set; }
    public  required SettingType Type { get; set; }
    public  required User User { get; set; }
}