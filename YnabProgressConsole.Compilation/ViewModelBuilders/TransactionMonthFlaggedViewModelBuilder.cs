using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class TransactionMonthFlaggedViewModelBuilder 
    : ViewModelBuilder<TransactionMonthFlaggedAggregator, IEnumerable<TransactionMonthFlaggedAggregate>>
{
    private List<string> _flagNames = [];
    
    protected override List<string> BuildColumnNames(IEnumerable<TransactionMonthFlaggedAggregate> aggregates)
    {
        _flagNames = aggregates
            .SelectMany(y => y.AmountAggregates)
            .Select(o => o.Flag)
            .Distinct()
            .ToList();
        
        return ColumnNames.Concat(_flagNames).ToList();
    }

    protected override List<List<object>> BuildRows(IEnumerable<TransactionMonthFlaggedAggregate> aggregates)
        => aggregates
            .Select(BuildIndividualRow)
            .Select(row => row.ToList())
            .ToList();

    private IEnumerable<object> BuildIndividualRow(TransactionMonthFlaggedAggregate aggregate)
    {
        yield return aggregate.Month;
        
        foreach (var flagName in _flagNames)
        {
            var flagAmountAggregate = aggregate.AmountAggregates
                .FirstOrDefault(amountAggregate => amountAggregate.Flag == flagName);

            if (flagAmountAggregate is null)
            {
                var displayableEmptyValue = AmountPercentageChangeDisplayFormatter.Format(0, 0);
                
                yield return displayableEmptyValue;
                continue;
            }
            
            var displayableValue = AmountPercentageChangeDisplayFormatter.Format(
                flagAmountAggregate.CurrentAmount,
                flagAmountAggregate.PercentageChange);
            
            yield return displayableValue;
        }
    }
}