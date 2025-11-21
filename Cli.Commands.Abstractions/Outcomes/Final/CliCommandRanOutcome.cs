namespace Cli.Commands.Abstractions.Outcomes.Final;

public class CliCommandRanOutcome(CliCommand command) : CliCommandOutcome(CliCommandOutcomeKind.Skippable)
{
    public CliCommand Command { get; } = command;
}