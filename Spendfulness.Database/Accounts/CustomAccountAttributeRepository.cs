using Microsoft.EntityFrameworkCore;

namespace Spendfulness.Database.Accounts;

public class CustomAccountAttributeRepository
{
    private readonly SpendfulnessDbContext _dbContext;

    public CustomAccountAttributeRepository(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CustomAccountAttributes?> Get(string ynabAccountName, CancellationToken cancellationToken = default)
        => _dbContext
            .CustomAccountAttributes
            .FirstOrDefaultAsync(attribute => 
                attribute.YnabAccountName.Equals(ynabAccountName) ||
                attribute.YnabAccountName.Contains(ynabAccountName),
                cancellationToken);
}