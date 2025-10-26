using Microsoft.EntityFrameworkCore;

namespace Spendfulness.Database.Users;

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
            .FirstAsync(u => u.Active);
}