using System.ComponentModel.DataAnnotations;

namespace YnabCli.Database.Settings;

public class SettingType
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public required string Name { get; set; }
}