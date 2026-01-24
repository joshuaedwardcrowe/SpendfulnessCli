using Microsoft.EntityFrameworkCore;

namespace Spendfulness.Database.Sqlite.Accounts;

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
    
    public async Task Save(CustomAccountAttributes customAccountAttributes, CancellationToken cancellationToken = default)
     => await _dbContext.CustomAccountAttributes.AddAsync(customAccountAttributes, cancellationToken);
}