namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandOutputOutcome(string output) : CliCommandOutcome(CliCommandOutcomeKind.Final)
{
    public string Output = output;
}