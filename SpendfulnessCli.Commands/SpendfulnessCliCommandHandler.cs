using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using SpendfulnessCli.Commands.Accounts;
using YnabSharp;

namespace SpendfulnessCli.Commands;

public abstract class SpendfulnessCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler
{
    protected readonly SpendfulnessBudgetClient SpendfulnessBudgetClient = spendfulnessBudgetClient;

    protected CliCommandOutcome[] OutcomeAs(Account account)
        => [new AccountCliCommandOutcome(account)];
}