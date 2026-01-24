using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using Spendfulness.Database.Sqlite.Settings;
using Spendfulness.Database.Sqlite.Users;

namespace SpendfulnessCli.Sync.Synchronisers;

public class DatabaseSynchroniser(SpendfulnessDbContext dbContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await dbContext.Database.EnsureCreatedAsync(stoppingToken);
        
        var activeUser = await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Name == "Joshua Crowe", stoppingToken);
        
        if (activeUser == null)
        {
            activeUser  = new User
            {
                Active = true,
                Name = "Joshua Crowe",
                Settings = new List<Setting>()
            };
            
            await dbContext.Users.AddAsync(activeUser, stoppingToken);
        }
        
        var ynabApiKeySettingType = await dbContext
            .SettingTypes
            .FirstAsync(x => x.Name == SettingTypeNames.YnabApiKey, cancellationToken: stoppingToken);

        var ynabApiKeySetting = await dbContext
            .Settings
            .FirstOrDefaultAsync(x => 
                    x.User == activeUser &&
                    x.Type == ynabApiKeySettingType, 
                stoppingToken);

        if (ynabApiKeySetting == null)
        {
            ynabApiKeySetting = new Setting
            {
                Type = ynabApiKeySettingType,
                User = activeUser,
                Value = string.Empty,
            };
            
            activeUser.Settings.Add(ynabApiKeySetting);
        }
        
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}