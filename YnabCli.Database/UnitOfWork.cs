using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using YnabCli.Database.Accounts;
using YnabCli.Database.Commitments;
using YnabCli.Database.Users;

namespace YnabCli.Database;

public class UnitOfWork(YnabCliDbContext ynabCliDbContext)
{
    public Task<User> GetActiveUser() => GetUserIncludable().FirstAsync(u => u.Active);
    
    public Task<List<CustomAccountType>> GetAccountTypes()
        => ynabCliDbContext
            .CustomAccountTypes
            .ToListAsync();
    
    public Task Save() => ynabCliDbContext.SaveChangesAsync();

    private IIncludableQueryable<User, ICollection<Commitment>> GetUserIncludable()
        => ynabCliDbContext
            .Users
            .Include(u => u.Settings)
            .ThenInclude(s => s.Type)
            .Include(u => u.Commitments);
}