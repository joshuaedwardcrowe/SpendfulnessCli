using ConsoleTables;
using Ynab;
using Ynab.Clients;
using Ynab.Connected;
using Ynab.Extensions;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Handlers;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly CommandBudgetGetter _budgetGetter;
    private readonly AmountViewModelBuilder _viewModelBuilder;

    public SpareMoneyCommandHandler(
        CommandBudgetGetter budgetGetter,
        AmountViewModelBuilder viewModelBuilder)
    {
        _budgetGetter = budgetGetter;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SpareMoneyCommand command, CancellationToken cancellationToken)
    {
        var budget =  await _budgetGetter.Get();

        var accounts = await budget.GetAccounts();
        var filteredAccounts = accounts.FilterByType(AccountType.Checking, AccountType.Savings);

        if (command.Add.HasValue)
        {
            var milliunit = MilliunitSanitiser.Desanitise(command.Add.Value);
            
            var placeholderResponse = new AccountResponse
            {
                Type = AccountType.Checking,
                ClearedBalance = milliunit
            };
            
            var placeholderAccount = new Account(placeholderResponse);

            filteredAccounts = filteredAccounts.Concat([placeholderAccount]);
        }

        if (command.MinusSavings.HasValue && command.MinusSavings.Value)
        {
            filteredAccounts = filteredAccounts.Where(account => account.Type == AccountType.Checking);
        }
    
        var criticalCategoryGroups = await GetCriticalCategoryGroups(budget);
    
        var aggregator = new CategoryDeductedBalanceAggregator(filteredAccounts, criticalCategoryGroups);

        _viewModelBuilder
            .AddAggregator(aggregator)
            .AddColumnNames(["Spare Money"])
            .AddRowCount(false);

        if (command.Minus.HasValue)
        {
            _viewModelBuilder.AddMinus(command.Minus.Value);
        }

        var viewModel = _viewModelBuilder.Build();

        return Compile(viewModel);
    }
    
    private static async Task<IEnumerable<CategoryGroup>> GetCriticalCategoryGroups(ConnectedBudget connectedBudget)
    {
        var criticalCategoryGroupNames = new List<string>
        {
            "Phone",
            "Career",
            "Owning a Home",
            "Maintaining a Home",
            "Credit Card Payments"
        };
        
        var categoryGroups = await connectedBudget.GetCategoryGroups();

        return categoryGroups.FilterTo(criticalCategoryGroupNames);
    }
}