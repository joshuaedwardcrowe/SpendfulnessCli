using KitCli.Abstractions.Aggregators;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using KitCli.Commands.Abstractions.Outcomes.Reusable;
using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Aggregation.Extensions;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using YnabSharp;
using YnabSharp.Extensions;

namespace SpendfulnessCli.Commands.Reporting.RecurringTransactions
{
    public class RecurringTransactionsCliCommandHandler(SpendfulnessBudgetClient budgetClient)
        : CliCommandHandler, ICliCommandHandler<RecurringTransactionsCliCommand>
    {
        private const int DefaultMinimumOccurrences = 2;
    
        public async Task<CliCommandOutcome[]> Handle(RecurringTransactionsCliCommand cliCommand, CancellationToken cancellationToken)
        {
            var aggregator = await PrepareAggregator(cliCommand);

            var table = new TransactionPayeeMemoOccurrenceCliTableBuilder()
                .WithAggregator(aggregator)
                .Build();

            return [
                new ListAggregatorCliCommandOutcome<TransactionPayeeMemoOccurrenceAggregate>(aggregator),
                new CliCommandTableOutcome(table)
            ];
        }

        private async Task<CliListAggregator<TransactionPayeeMemoOccurrenceAggregate>> PrepareAggregator(RecurringTransactionsCliCommand cliCommand)
        {
            var budget =  await budgetClient.GetDefaultBudget();
            var transactions = await budget.GetTransactions();
        
            var aggregator = new TransactionPayeeMemoOccurrenceYnabListAggregator(transactions);
            
            aggregator.BeforeAggregation(ts => ts.FilterOutCategories([YnabConstants.SplitCategoryId]));

            if (cliCommand.From.HasValue)
            {
                aggregator.BeforeAggregation(ts => ts.FilterFrom(cliCommand.From.Value));
            }
        
            if (cliCommand.To.HasValue)
            {
                aggregator.BeforeAggregation(ts => ts.FilterTo(cliCommand.To.Value));
            }

            if (cliCommand.PayeeName != null)
            {
                aggregator.BeforeAggregation(ts => ts.FilterToPayeeNames(cliCommand.PayeeName));
            }

            return aggregator
                .AfterAggregation(a => a.FilterToMinimumOccurrences(cliCommand.MinimumOccurrences ?? DefaultMinimumOccurrences))
                .AfterAggregation(a => a.OrderByDescending(aggregate => aggregate.MemoOccurrence));
        }
    }
}