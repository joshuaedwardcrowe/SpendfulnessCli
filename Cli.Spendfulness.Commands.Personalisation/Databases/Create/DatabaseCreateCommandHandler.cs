using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Databases.Create;

public class DatabaseCreateCommandHandler : CommandHandler, ICommandHandler<DatabaseCreateCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public DatabaseCreateCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(DatabaseCreateCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

        return Compile("Database exists");
    }
}