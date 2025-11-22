using Cli.Commands.Abstractions.Outcomes;
using Ynab;

namespace SpendfulnessCli.Commands;

public class AccountCliCommandOutcome(Account account) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public Account Account { get; set; } = account;
}