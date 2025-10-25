using Cli.Spendfulness.Database;

namespace Cli.Spendfuness.Database.Abstractions;

public class YnabCliDbException(YnabCliDbExceptionCode code, string message)
    : CliException(CliExceptionCode.Custom, message)
{
    public new YnabCliDbExceptionCode Code = code;
}