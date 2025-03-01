using YnabCli.Database.Accounts;
using YnabCli.Database.Commitments;
using YnabCli.Database.Settings;

namespace YnabCli.Database.Users;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool Active { get; set; }
    public ICollection<Setting> Settings { get; set; }
        = new List<Setting>();
    public ICollection<Commitment> Commitments { get; set; }
        = new List<Commitment>();
    public ICollection<AccountAttributes> CustomAccountTypes { get; set; }
        = new List<AccountAttributes>();

    public string? YnabApiKey => Settings.AsString(SettingTypeNames.YnabApiKey);
    
    public Guid? DefaultBudgetId => Settings.AsGuid(SettingTypeNames.DefaultBudgetId);

    public int? SyncFrequency => Settings.AsInt(SettingTypeNames.SyncFrequencyInHours);
}