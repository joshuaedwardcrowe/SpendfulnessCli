using Cli.Spendfulness.Database.Accounts;
using Cli.Spendfulness.Database.SpendingSamples;
using Cli.Spendfulness.Database.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cli.Spendfulness.Database;

public class YnabCliDb(YnabCliDbContext ynabCliDbContext)
{
    public readonly YnabCliDbContext Context = ynabCliDbContext;
    
    public Task<User> GetActiveUser() => 
        Context
            .Users
            .Include(u => u.Settings)
            .ThenInclude(s => s.Type)
            .Include(u => u.Commitments)
            .FirstAsync(u => u.Active);
    
    public Task<List<CustomAccountType>> GetAccountTypes()
        => Context
            .CustomAccountTypes
            .ToListAsync();

    public Task<List<string>> GetDerivedSpendingSampleTransactionIds()
        => Context
            .SpendingSamples
            .Where(ss => ss.YnabTransactionId != null)
            .Select(ss => ss.YnabTransactionId!)
            .ToListAsync();
    
    public Task<List<SpendingSampleMatchCriteria>> GetSpendingSampleMatchCriteria()
        => Context
            .SpendingSampleMatchCriteria
            .Include(ss => ss.Prices)
            .ToListAsync();
    
    public async Task Sync<TEntity>() where TEntity : class
    {
        var recentlyUsedEntries = Context.ChangeTracker.Entries();

        var relevantEntities = recentlyUsedEntries.Where(o => o.Entity is TEntity);

        var reloadsToCarryOut = new List<EntityEntry>(relevantEntities.ToList())
            .Select(o => o.ReloadAsync());

        await Task.WhenAll(reloadsToCarryOut);
    }

    public Task Save() => Context.SaveChangesAsync();
}