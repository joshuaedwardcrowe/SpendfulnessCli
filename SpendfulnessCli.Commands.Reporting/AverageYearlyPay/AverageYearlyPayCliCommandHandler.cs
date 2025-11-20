using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayCliCommandHandler(SpendfulnessBudgetClient budgetClient)
    : CliCommandHandler, ICliCommandHandler<AverageYearlyPayCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(AverageYearlyPayCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator();

        var viewModel = new TransactionYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return OutcomeAs(viewModel);
    }

    private async Task<YnabListAggregator<TransactionYearAverageAggregate>> PrepareAggregator()
    {
        var budget =  await budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionAveragePerYearYnabListAggregator(transactions);

        aggregator
            .BeforeAggregation(t => t.FilterToInflow())
            .BeforeAggregation(t => t.FilterToPayeeNames("BrightHR"))
            .BeforeAggregation(t => t.OrderByYear()); 

        return aggregator;
    }
}
