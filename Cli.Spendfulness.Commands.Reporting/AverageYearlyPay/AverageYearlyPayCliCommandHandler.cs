using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayCliCommandHandler(SpendfulnessBudgetClient budgetClient)
    : CliCommandHandler, ICliCommandHandler<AverageYearlyPayCliCommand>
{
    public async Task<CliCommandOutcome> Handle(AverageYearlyPayCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator();

        var viewModel = new TransactionYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListYnabAggregator<TransactionYearAverageAggregate>> PrepareAggregator()
    {
        var budget =  await budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionAveragePerYearYnabAggregator(transactions);

        aggregator
            .BeforeAggregation(t => t.FilterToInflow())
            .BeforeAggregation(t => t.FilterToPayeeNames("BrightHR"))
            .BeforeAggregation(t => t.OrderByYear()); 

        return aggregator;
    }
}
