using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Microsoft.Extensions.Hosting;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.Settings;
using Spendfulness.Database.Users;

namespace YnabCli.Sync.Synchronisers;

public class DatabaseSynchroniser(SpendfulnessDbContext dbContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await dbContext.Database.EnsureCreatedAsync(stoppingToken);
        
        var potentiallyActiveUser = await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Name == "Joshua Crowe", stoppingToken);
        
        if (potentiallyActiveUser == null)
        {
            var ynabApiKeySettingType = await dbContext
                .SettingTypes
                .FirstAsync(x => x.Name == SettingTypeNames.YnabApiKey, cancellationToken: stoppingToken);

            var activeUser = new User
            {
                Active = true,
                Name = "Joshua Crowe",
                Settings = new List<Setting>()
            };

            var apiKeySetting = new Setting()
            {
                Type = ynabApiKeySettingType,
                Value = string.Empty,
                User = activeUser
            };
        
            activeUser.Settings.Add(apiKeySetting);
            await dbContext.Users.AddAsync(activeUser, stoppingToken);
        }


        var potentiallyCustomAccountType = await dbContext
            .CustomAccountTypes
            .FirstOrDefaultAsync(x => x.Name == "American Express Rewards", cancellationToken: stoppingToken);

        if (potentiallyCustomAccountType == null)
        {
            var customAccountType = new CustomAccountType
            {
                Name = "American Express Rewards"
            };
            
            await dbContext.CustomAccountTypes.AddAsync(customAccountType, stoppingToken);
        }
        
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}