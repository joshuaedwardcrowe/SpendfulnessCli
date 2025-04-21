using ConsoleTables;
using Ynab;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregator;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.FlagChanges;

public class FlagChangesCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<FlagChangesCommand>
{
    public async Task<ConsoleTable> Handle(FlagChangesCommand command, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthFlaggedAggregator(categoryGroups, transactions);
            
        if (command.From.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterFrom(command.From.Value));
        }

        if (command.To.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterTo(command.To.Value));
        }
        
        var viewModel = new TransactionMonthFlaggedViewModelBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}