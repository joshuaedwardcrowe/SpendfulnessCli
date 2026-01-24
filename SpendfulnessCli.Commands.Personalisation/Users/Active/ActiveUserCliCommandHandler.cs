using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;

namespace SpendfulnessCli.Commands.Personalisation.Users.Active;

public class ActiveUserCliCommandHandler : CliCommandHandler, ICliCommandHandler<ActiveUserCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public ActiveUserCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(ActiveUserCliCommand request, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active);
        
        return activeUser != null
            ? OutcomeAs($"Active user is {activeUser.Name}")
            : OutcomeAs($"No active user.");
    }
}