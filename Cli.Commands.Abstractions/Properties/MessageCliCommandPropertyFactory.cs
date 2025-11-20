using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;

namespace Cli.Commands.Abstractions.Properties;

public class MessageCliCommandPropertyFactory : ICliCommandPropertyFactory
{
    public bool CanCreateProperty(CliCommandOutcome outcome) => outcome is CliCommandMessageOutcome;

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandMessageOutcome messageOutcome)
        {
            return new MessageCliCommandProperty(messageOutcome.Message);
        }

        throw new InvalidOperationException("Cannot create property from the given outcome.");
    }
}