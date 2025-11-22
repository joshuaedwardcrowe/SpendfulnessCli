using Cli.Commands.Abstractions.Outcomes;
using Ynab;

namespace SpendfulnessCli.Commands.Accounts;

public class AccountCliCommandOutcome(Account account) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public Account Account { get; set; } = account;
}