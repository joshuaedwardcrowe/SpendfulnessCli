namespace Cli;

public class CliWorkflowException(CliWorkflowExceptionCode code, string message)
    : CliException(CliExceptionCode.Command, message)
{
    public new CliWorkflowExceptionCode Code = code;
}