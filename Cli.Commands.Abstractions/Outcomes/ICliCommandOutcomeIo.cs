using Cli.Commands.Abstractions.Io;

namespace Cli.Commands.Abstractions.Outcomes;

public interface ICliCommandOutcomeIo : ICliIo
{
    void Say(CliCommandOutcome[] outcomes);
}