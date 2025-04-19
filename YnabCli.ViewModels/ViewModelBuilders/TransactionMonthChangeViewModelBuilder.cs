using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Aggregator.ListAggregators;
using YnabCli.ViewModels.Formatters;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

public class TransactionMonthChangeViewModelBuilder
    : ViewModelBuilder<ListAggregator<TransactionMonthTotalAggregate>, IEnumerable<TransactionMonthTotalAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<TransactionMonthTotalAggregate> evaluation)
        => TransactionMonthChangeViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<TransactionMonthTotalAggregate> aggregates)
        => aggregates
            .Select(BuildIndividualRow)
            .Select(row => row.ToList())
            .ToList();
    
    private IEnumerable<object> BuildIndividualRow(TransactionMonthTotalAggregate aggregate)
    {
        yield return aggregate.Month;
        
        var displayableTotalAmount = CurrencyDisplayFormatter.Format(aggregate.TotalAmount);
        yield return displayableTotalAmount;

        var displayablePercentageChangeAmount = PercentageDisplayFormatter.Format(aggregate.PercentageChange);
        yield return displayablePercentageChangeAmount;
    }
}