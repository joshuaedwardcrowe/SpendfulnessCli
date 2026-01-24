using System.ComponentModel.DataAnnotations;
using Spendfulness.Database.Sqlite.Users;

namespace Spendfulness.Database.Sqlite.Settings;

public class Setting
{
    public int Id { get; set; }

    [MaxLength(2000)]
    public required string Value { get; set; }

    public required SettingType Type { get; set; }

    public required User User { get; set; }
}