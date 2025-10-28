namespace Cli.Instructions.Abstractions;

public class NoInstructionPrefixException : CliInstructionException
{
    public NoInstructionPrefixException(string message)
        : base(CliInstructionExceptionCode.NoInstructionPrefix, message)
    {
    }
}
