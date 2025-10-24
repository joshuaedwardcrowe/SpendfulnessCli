using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.FlagChanges;

public class FlagChangesCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<FlagChangesCommand>
{
    public async Task<CliCommandOutcome> Handle(FlagChangesCommand command, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthFlaggedYnabAggregator(categoryGroups, transactions);
            
        if (command.From.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterFrom(command.From.Value));
        }

        if (command.To.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterTo(command.To.Value));
        }
        
        var viewModel = new TransactionMonthFlaggedCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}