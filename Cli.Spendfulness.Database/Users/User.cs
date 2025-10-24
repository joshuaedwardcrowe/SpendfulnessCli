using System.ComponentModel.DataAnnotations;
using Cli.Spendfulness.Database.Accounts;
using Cli.Spendfulness.Database.Commitments;
using Cli.Spendfulness.Database.Settings;

namespace Cli.Spendfulness.Database.Users;

public class User
{
    public int Id { get; set; }
    [MaxLength(1000)]
    public required string Name { get; set; }
    public bool Active { get; set; }
    public ICollection<Setting> Settings { get; set; } = new List<Setting>();
    public ICollection<Commitment> Commitments { get; set; } = new List<Commitment>();
    public ICollection<AccountAttributes> AccountAttributes { get; set; } = new List<AccountAttributes>();
    public string? YnabApiKey => Settings.AsString(SettingTypeNames.YnabApiKey);
    
    public Guid? DefaultBudgetId => Settings.AsGuid(SettingTypeNames.DefaultBudgetId);

    public int? SyncFrequency => Settings.AsInt(SettingTypeNames.SyncFrequencyInHours);
}