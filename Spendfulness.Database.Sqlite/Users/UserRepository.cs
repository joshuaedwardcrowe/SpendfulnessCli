using Microsoft.EntityFrameworkCore;

namespace Spendfulness.Database.Sqlite.Users;

public class UserRepository
{
    private readonly SpendfulnessDbContext _dbContext;

    public UserRepository(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<User> FindActiveUser() 
        => _dbContext
            .Users
            .Include(user => user.Settings)
            .ThenInclude(setting => setting.Type)
            .FirstAsync(u => u.Active);
}