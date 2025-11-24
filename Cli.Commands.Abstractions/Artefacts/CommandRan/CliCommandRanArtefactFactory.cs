using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Artefacts.CommandRan;

public class CliCommandRanArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is CliCommandRanOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        if (outcome is not CliCommandRanOutcome ranOutcome)
            throw new InvalidOperationException("Cannot create CliCommandRanProperty from the given outcome.");

        return new RanCliCommandArtefact(ranOutcome.Command);
    }
}