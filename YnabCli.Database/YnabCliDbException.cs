using YnabCli.Abstractions;

namespace YnabCli.Database;

public class YnabCliDbException : YnabCliException
{
    public YnabCliDbExceptionCode Code { get; }
    
    public YnabCliDbException(YnabCliDbExceptionCode code, string message) : base(message)
    {
        Code = code;
    }
}