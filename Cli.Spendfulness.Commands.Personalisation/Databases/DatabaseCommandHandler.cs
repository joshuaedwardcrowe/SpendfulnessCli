using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Spendfulness.Commands.Personalisation.Databases;

public class DatabaseCommandHandler : ICommandHandler<DatabaseCommand>
{
    public Task<CliCommandOutcome> Handle(DatabaseCommand request, CancellationToken cancellationToken)
    {
        throw new Exception("No functionality in the base command, please use a subcommand");
    }
}