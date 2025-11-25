namespace Cli.Commands.Abstractions.Outcomes.Final;

public class RanCliCommandOutcome(CliCommand command) : CliCommandOutcome(CliCommandOutcomeKind.Skippable)
{
    public CliCommand Command { get; } = command;
}