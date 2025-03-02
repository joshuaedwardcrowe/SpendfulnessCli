using ConsoleTables;
using Ynab;
using Ynab.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.FlagChanges;

public class FlagChangesCommandHandler : CommandHandler, ICommandHandler<FlagChangesCommand>
{
    private readonly DbBudgetClient _budgetClient;
    private readonly TransactionMonthFlaggedViewModelBuilder _viewModelBuilder;

    public FlagChangesCommandHandler(
        DbBudgetClient budgetClient,
        TransactionMonthFlaggedViewModelBuilder viewModelBuilder)
    {
        _budgetClient = budgetClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(FlagChangesCommand command, CancellationToken cancellationToken)
    {
        var budget = await _budgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();

        var castedTransactions = transactions.Cast<Transaction>();

        if (command.From.HasValue)
        {
            castedTransactions = castedTransactions
                .FilterFrom(command.From.Value);
        }

        if (command.To.HasValue)
        {
            castedTransactions = castedTransactions.FilterTo(command.To.Value);
        }
        
        var aggregator = new TransactionMonthFlaggedAggregator(categoryGroups, castedTransactions);
        
        var viewModel = _viewModelBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}