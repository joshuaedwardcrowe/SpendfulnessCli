namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandOutputOutcome(string output) : CliCommandOutcome
{
    public string Output = output;
}