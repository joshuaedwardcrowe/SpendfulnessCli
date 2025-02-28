using YnabCli.Abstractions;

namespace YnabCli.Commands.Exceptions;

public class CommandException : YnabCliException
{
    public CommandExceptionCode Code { get; }
    
    public CommandException(CommandExceptionCode code, string message) : base(message)
    {
        Code = code;
    }
}