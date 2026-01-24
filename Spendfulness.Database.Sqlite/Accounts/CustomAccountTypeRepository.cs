using Microsoft.EntityFrameworkCore;

namespace Spendfulness.Database.Sqlite.Accounts;

public class CustomAccountTypeRepository
{
    private readonly SpendfulnessDbContext _dbContext;

    public CustomAccountTypeRepository(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CustomAccountType> Find(string customAccountTypeName, CancellationToken cancellationToken)
        => _dbContext
            .CustomAccountTypes
            .FirstAsync(x => x.Name == customAccountTypeName, cancellationToken);
    
    public Task<CustomAccountType> Get(string customAccountTypeName, CancellationToken cancellationToken)
        => _dbContext
            .CustomAccountTypes
            .FirstAsync(x => x.Name == customAccountTypeName, cancellationToken);
}