namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandExceptionOutcome(Exception exception) : CliCommandOutcome
{
    public Exception Exception { get; set; } = exception;
}