using YnabCli.Abstractions;

namespace YnabCli.Instructions.Exceptions;

public class InstructionException : YnabCliException
{
    public InstructionExceptionCode Code { get; }
    
    public InstructionException(InstructionExceptionCode code, string message) : base(message)
    {
        Code = code;
    }
}