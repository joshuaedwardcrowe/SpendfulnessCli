using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Ynab;

namespace SpendfulnessCli.Commands;

public abstract class SpendfulnessCliCommandHandler : CliCommandHandler
{
    protected readonly SpendfulnessBudgetClient SpendfulnessBudgetClient;

    protected SpendfulnessCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    {
        SpendfulnessBudgetClient = spendfulnessBudgetClient;
    }
    
    protected CliCommandOutcome[] OutcomeAs(Account account)
        => [new AccountCliCommandOutcome(account)];
}