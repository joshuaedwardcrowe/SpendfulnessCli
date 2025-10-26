using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Spendfulness.Database;

public static class DbContextExtensions
{
    public static async Task Sync<TEntity>(this DbContext dbContext) where TEntity : class
    {
        var recentlyUsedEntries = dbContext.ChangeTracker.Entries();

        var relevantEntities = recentlyUsedEntries.Where(o => o.Entity is TEntity);

        var reloadsToCarryOut = new List<EntityEntry>(relevantEntities.ToList())
            .Select(o => o.ReloadAsync());

        await Task.WhenAll(reloadsToCarryOut);
    }
}