using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregator;
using SpendfulnessCli.Aggregation.Aggregator.AmountAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab;
using Ynab.Extensions;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;

namespace SpendfulnessCli.Commands.Reporting.SpareMoney;

public class SpareMoneyCliCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<SpareMoneyCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SpareMoneyCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(cliCommand);
        
        var viewModelBuilder = new AmountCliTableBuilder();
        viewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false);

        if (cliCommand.Minus.HasValue)
        {
            viewModelBuilder.WithSubtraction(cliCommand.Minus.Value);
        }

        var table = viewModelBuilder.Build();
        table.Columns = ["Spare Money"];

        return
        [
            new CliCommandTableOutcome(table),
            new CliCommandMessageOutcome("Ran the Spare Money command")
        ];
    }

    private async Task<YnabAggregator<decimal>> PrepareAggregator(SpareMoneyCliCommand cliCommand)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var accounts = await budget.GetAccounts();
        var categoryGroups = await budget.GetCategoryGroups();

        var aggregator = new CategoryDeductedAmountYnabAggregator(accounts, categoryGroups)
            .BeforeAggregation(a => a.FilterToTypes(AccountType.Checking, AccountType.Savings))
            .BeforeAggregation(FilterToCriticalCategoryGroups);

        if (cliCommand.Add.HasValue)
        {
            aggregator.BeforeAggregation(a => AddPlaceholderAccountForAddition(a, cliCommand.Add.Value));
        }

        if (cliCommand.MinusSavings.HasValue && cliCommand.MinusSavings.Value)
        {
            aggregator.BeforeAggregation(a => a.FilterToTypes(AccountType.Checking));
        }

        return aggregator;
    }

    private IEnumerable<Account> AddPlaceholderAccountForAddition(IEnumerable<Account> accounts, decimal amount)
    {
        var milliunit = MilliunitConverter.PoundsToMilliunit(amount);
        
        var placeholderResponse = new AccountResponse
        {
            Id = Guid.Empty,
            Name = nameof(SpareMoneyCliCommand),
            Type = AccountType.Checking,
            ClearedBalance = milliunit,
            OnBudget = false,
            Closed = false
        };
        
        var account = new Account(placeholderResponse);
        
        return accounts.Append(account);
    }

    private IEnumerable<CategoryGroup> FilterToCriticalCategoryGroups(IEnumerable<CategoryGroup> categoryGroups)
    {
        // TODO: This should really come from the db.
        var criticalCategoryGroupNames = new List<string>
        {
            "Phone",
            "Career",
            "Owning a Home",
            "Maintaining a Home",
            "Credit Card Payments"
        };

        return categoryGroups.FilterTo(criticalCategoryGroupNames);
    }
}