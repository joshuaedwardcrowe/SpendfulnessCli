using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;

namespace SpendfulnessCli.Commands;

public class AccountCliCommandPropertyFactory : ICliCommandPropertyFactory
{
    public bool CanCreatePropertyWhen(CliCommandOutcome outcome)
        => outcome is AccountCliCommandOutcome;

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        var accountOutcome = (AccountCliCommandOutcome)outcome;
        return new AccountCliCommandProperty(accountOutcome.Account);
    }
}