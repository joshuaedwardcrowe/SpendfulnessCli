using ConsoleTables;
using Ynab;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Aggregator;
using YnabProgressConsole.Compilation.ViewModelBuilders;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly AmountViewModelBuilder _viewModelBuilder;

    public SpareMoneyCommandHandler(BudgetsClient budgetsClient, AmountViewModelBuilder viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SpareMoneyCommand command, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
    
        var budget = budgets.First();

        var accounts = await budget.GetAccounts();
        var filteredAccounts = accounts.FilterByType(AccountType.Checking, AccountType.Savings);

        if (command.MinusSavings.HasValue && command.MinusSavings.Value)
        {
            filteredAccounts = filteredAccounts.Where(account => account.Type == AccountType.Checking);
        }
    
        var criticalCategoryGroups = await GetCriticalCategoryGroups(budget);
    
        var aggregator = new CategoryDeductedBalanceAggregator(filteredAccounts, criticalCategoryGroups);
    
        var viewModel = _viewModelBuilder
            .AddAggregator(aggregator)
            .AddColumnNames(["Spare Money"])
            .AddRowCount(false)
            .Build();
    
        return Compile(viewModel);
    }
    
    private static async Task<IEnumerable<CategoryGroup>> GetCriticalCategoryGroups(Budget budget)
    {
        var criticalCategoryGroupNames = new List<string>
        {
            "Phone",
            "Career",
            "Owning a Home",
            "Maintaining a Home",
            "Credit Card Payments"
        };
        
        var categoryGroups = await budget.GetCategoryGroups();

        return categoryGroups.FilterTo(criticalCategoryGroupNames);
    }
}