using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Databases.Create;

public class DatabaseCreateCliCliCommandHandler : CliCommandHandler, ICliCommandHandler<DatabaseCreateCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public DatabaseCreateCliCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(DatabaseCreateCliCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

        return Compile("Database exists");
    }
}