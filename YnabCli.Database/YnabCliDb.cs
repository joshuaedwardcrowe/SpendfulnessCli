using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using YnabCli.Database.Accounts;
using YnabCli.Database.Commitments;
using YnabCli.Database.SpendingSamples;
using YnabCli.Database.Users;

namespace YnabCli.Database;

public class YnabCliDb(YnabCliDbContext ynabCliDbContext)
{
    public readonly YnabCliDbContext Context = ynabCliDbContext;
    
    public Task<User> GetActiveUser() => GetUserIncludable().FirstAsync(u => u.Active);
    
    public Task<List<CustomAccountType>> GetAccountTypes()
        => Context
            .CustomAccountTypes
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

    private IIncludableQueryable<User, ICollection<Commitment>> GetUserIncludable()
        => Context
            .Users
            .Include(u => u.Settings)
            .ThenInclude(s => s.Type)
            .Include(u => u.Commitments);
}