using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Properties.CommandRan;

public class CliCommandRanPropertyFactory : ICliCommandPropertyFactory
{
    public bool CanCreatePropertyWhen(CliCommandOutcome outcome)
        => outcome is CliCommandRanOutcome;

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is not CliCommandRanOutcome ranOutcome)
            throw new InvalidOperationException("Cannot create CliCommandRanProperty from the given outcome.");

        return new CliCommandRanProperty(ranOutcome.Command);
    }
}