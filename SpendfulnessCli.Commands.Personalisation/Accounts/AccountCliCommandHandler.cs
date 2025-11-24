using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Accounts;

public class AccountCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : SpendfulnessCliCommandHandler(spendfulnessBudgetClient), ICliCommandHandler<AccountCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(AccountCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await SpendfulnessBudgetClient.GetDefaultBudget();
        
        var account = await budget.GetAccount(request.AccountId);

        return OutcomeAs(account);
    }
}