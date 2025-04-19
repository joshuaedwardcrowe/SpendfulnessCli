using ConsoleTables;
using Ynab;
using Ynab.Connected;
using Ynab.Extensions;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;
using YnabCli.Aggregation.Aggregator.AmountAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly ConfiguredBudgetClient _budgetClient;
    private readonly AmountViewModelBuilder _viewModelBuilder;

    public SpareMoneyCommandHandler(
        ConfiguredBudgetClient budgetClient,
        AmountViewModelBuilder viewModelBuilder)
    {
        _budgetClient = budgetClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SpareMoneyCommand command, CancellationToken cancellationToken)
    {
        var budget =  await _budgetClient.GetDefaultBudget();

        var accounts = await budget.GetAccounts();
        var filteredAccounts = accounts.FilterByType(AccountType.Checking, AccountType.Savings);

        if (command.Add.HasValue)
        {
            var milliunit = MilliunitSanitiser.Desanitise(command.Add.Value);
            
            var placeholderResponse = new AccountResponse
            {
                Id = Guid.Empty,
                Name = nameof(SpareMoneyCommand),
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
    
        var aggregator = new CategoryDeductedAmountAggregator(filteredAccounts, criticalCategoryGroups);

        _viewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false);

        if (command.Minus.HasValue)
        {
            _viewModelBuilder.WithSubtraction(command.Minus.Value);
        }

        var viewModel = _viewModelBuilder.Build();
        viewModel.Columns = ["Spare Money"];

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