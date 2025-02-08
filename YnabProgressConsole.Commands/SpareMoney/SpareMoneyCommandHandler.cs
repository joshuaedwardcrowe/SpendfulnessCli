using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Aggregates;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.SpareMoney;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate> _aggregateViewModelBuilder;

    public SpareMoneyCommandHandler(BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(SpareMoneyAggregation))]
        IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate> aggregateViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _aggregateViewModelBuilder = aggregateViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SpareMoneyCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();

        var budget = budgets.First();
        
        // TODO: This needs moving to some kind of config or setting.
        var criticalCategoryGroupNames = new List<string>
        {
            "Phone",
            "Career",
            "Owning a Home",
            "Maintaining a Home",
            "Credit Card Payments"
        };
        
        var categoryGroups = await budget.GetCategoryGroups();
        var criticalCategoryGroups = categoryGroups
            .FilterTo(criticalCategoryGroupNames.ToArray())
            .Sum(o => o.Balance);

        var accounts = await budget.GetAccounts();
        var checkingAccounts = accounts
            .FilterToChecking()
            .AggregateByBalance()
            .IncludeAmountToIgnore(criticalCategoryGroups);

        var columnNames = SpareMoneyViewModel.GetColumnNames();
        
        var viewModel = _aggregateViewModelBuilder
            .AddAggregation(checkingAccounts)
            .AddColumnNames(columnNames.ToArray())
            .Build();
        
        return Compile(viewModel);
    }
}