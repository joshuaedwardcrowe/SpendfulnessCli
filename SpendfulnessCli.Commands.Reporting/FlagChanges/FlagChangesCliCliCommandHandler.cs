using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregator;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reporting.FlagChanges;

public class FlagChangesCliCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<FlagChangesCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(FlagChangesCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthFlaggedYnabAggregator(categoryGroups, transactions);
            
        if (cliCommand.From.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterFrom(cliCommand.From.Value));
        }

        if (cliCommand.To.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterTo(cliCommand.To.Value));
        }
        
        var viewModel = new TransactionMonthFlaggedCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return OutcomeAs(viewModel);
    }
}