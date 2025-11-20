using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Databases.Create;

public class DatabaseCreateCliCommandHandler : CliCommandHandler, ICliCommandHandler<DatabaseCreateCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public DatabaseCreateCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(DatabaseCreateCliCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

        return OutcomeAs("Database exists");
    }
}