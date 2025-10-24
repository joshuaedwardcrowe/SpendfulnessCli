using Ynab.Exceptions;

namespace YnabCli;

public class UnknownCliCommandOutcomeException(string message)
    : YnabException(YnabExceptionCode.UnknownCliCommandOutcome, message)
{
}