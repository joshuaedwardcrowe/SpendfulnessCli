using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Commands.Abstractions.Io.Outcomes;

public interface ICliCommandOutcomeIo : ICliIo
{
    void Say(CliCommandOutcome[] outcomes);
}