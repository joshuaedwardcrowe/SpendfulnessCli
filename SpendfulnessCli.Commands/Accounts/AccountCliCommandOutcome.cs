using KitCli.Commands.Abstractions.Outcomes;
using YnabSharp;

namespace SpendfulnessCli.Commands.Accounts;

public class AccountCliCommandOutcome(Account account) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public Account Account { get; set; } = account;
}