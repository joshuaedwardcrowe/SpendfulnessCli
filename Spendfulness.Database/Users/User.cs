using System.ComponentModel.DataAnnotations;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.Commitments;
using Spendfulness.Database.Settings;

namespace Spendfulness.Database.Users;

public class User
{
    public int Id { get; set; }
    [MaxLength(1000)]
    public required string Name { get; set; }
    public bool Active { get; set; }
    public ICollection<Setting> Settings { get; set; } = new List<Setting>();
    public ICollection<Commitment> Commitments { get; set; } = new List<Commitment>();
    public string? YnabApiKey => Settings.AsString(SettingTypeNames.YnabApiKey);
    
    public Guid? DefaultBudgetId => Settings.AsGuid(SettingTypeNames.DefaultBudgetId);

    public int? SyncFrequency => Settings.AsInt(SettingTypeNames.SyncFrequencyInHours);
}