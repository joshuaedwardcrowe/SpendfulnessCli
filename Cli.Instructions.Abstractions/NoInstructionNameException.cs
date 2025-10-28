namespace Cli.Instructions.Abstractions;

public class NoInstructionNameException : CliInstructionException
{
    public NoInstructionNameException(string message)
        : base(CliInstructionExceptionCode.NoInstructionName, message)
    {
    }
}
