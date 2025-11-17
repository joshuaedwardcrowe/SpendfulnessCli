namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandOutputOutcome(string output) : CliCommandOutcome(CliCommandOutcomeKind.Output)
{
    public string Output = output;
}