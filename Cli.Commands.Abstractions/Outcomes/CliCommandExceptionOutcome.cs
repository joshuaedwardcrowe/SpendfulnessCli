namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandExceptionOutcome(Exception exception) : CliCommandOutcome(CliCommandOutcomeKind.Final)
{
    public Exception Exception { get; set; } = exception;
}