using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.SpendingSamples;
using Spendfulness.Database.Users;

namespace Spendfulness.Database;

[Obsolete("Please use repository pattern")]
public class SpendfulnessDb(SpendfulnessDbContext spendfulnessDbContext)
{
    public readonly SpendfulnessDbContext Context = spendfulnessDbContext;
    
    public Task<User> GetActiveUser() => 
        Context
            .Users
            .Include(u => u.Settings)
            .ThenInclude(s => s.Type)
            .Include(u => u.Commitments)
            .FirstAsync(u => u.Active);

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