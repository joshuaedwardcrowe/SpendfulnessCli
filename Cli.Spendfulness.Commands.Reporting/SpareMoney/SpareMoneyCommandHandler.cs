using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator;
using Cli.Spendfulness.Aggregation.Aggregator.AmountAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;
using Ynab;
using Ynab.Extensions;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;

namespace Cli.Ynab.Commands.Reporting.SpareMoney;

public class SpareMoneyCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    public async Task<CliCommandOutcome> Handle(SpareMoneyCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);
        
        var viewModelBuilder = new AmountCliTableBuilder();
        viewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false);

        if (command.Minus.HasValue)
        {
            viewModelBuilder.WithSubtraction(command.Minus.Value);
        }

        var viewModel = viewModelBuilder.Build();
        viewModel.Columns = ["Spare Money"];

        return Compile(viewModel);
    }

    private async Task<YnabAggregator<decimal>> PrepareAggregator(SpareMoneyCommand command)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
        var accounts = await budget.GetAccounts();
        var categoryGroups = await budget.GetCategoryGroups();

        var aggregator = new CategoryDeductedAmountYnabAggregator(accounts, categoryGroups)
            .BeforeAggregation(a => a.FilterToTypes(AccountType.Checking, AccountType.Savings))
            .BeforeAggregation(FilterToCriticalCategoryGroups);

        if (command.Add.HasValue)
        {
            aggregator.BeforeAggregation(a => AddPlaceholderAccountForAddition(a, command.Add.Value));
        }

        if (command.MinusSavings.HasValue && command.MinusSavings.Value)
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
            Name = nameof(SpareMoneyCommand),
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