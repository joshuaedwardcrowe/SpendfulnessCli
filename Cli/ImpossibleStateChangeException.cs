namespace Cli;

public class ImpossibleStateChangeException(string message)
    : CliWorkflowException(CliWorkflowExceptionCode.ImpossibleStateChange, message)
{
}