using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.Aggregation.Extensions;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.RecurringTransactions
{
    public class RecurringTransactionsCliCommandHandler(SpendfulnessBudgetClient budgetClient)
        : CliCommandHandler, ICliCommandHandler<RecurringTransactionsCliCommand>
    {
        private const int DefaultMinimumOccurrences = 2;
    
        public async Task<CliCommandOutcome> Handle(RecurringTransactionsCliCommand cliCommand, CancellationToken cancellationToken)
        {
            var aggregator = await PrepareAggregator(cliCommand);

            var viewModel = new TransactionPayeeMemoOccurrenceCliTableBuilder()
                .WithAggregator(aggregator)
                .Build();

            return Compile(viewModel);
        }

        private async Task<ListYnabAggregator<TransactionPayeeMemoOccurrenceAggregate>> PrepareAggregator(RecurringTransactionsCliCommand cliCommand)
        {
            var budget =  await budgetClient.GetDefaultBudget();
            var transactions = await budget.GetTransactions();
        
            var aggregator = new TransactionPayeeMemoOccurrenceYnabAggregator(transactions);
            
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