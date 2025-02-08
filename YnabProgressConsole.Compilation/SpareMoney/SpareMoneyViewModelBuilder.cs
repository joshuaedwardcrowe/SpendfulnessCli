using Ynab.Aggregates;

namespace YnabProgressConsole.Compilation.SpareMoney;

public class SpareMoneyViewModelBuilder 
    : ViewModelBuilder, IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate>
{
    private SpareMoneyAggregation _spareMoneyAggregation = new();
    
    public IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate> AddAggregation(SpareMoneyAggregation aggregate)
    {
        _spareMoneyAggregation = aggregate;
        return this;
    }

    public ViewModel Build()
    {
        var availableBalance = _spareMoneyAggregation.Aggregation.Sum(o => o.Balance);
        var balanceAfterDeduction = availableBalance - _spareMoneyAggregation.AmountToDeduct;
        
        return new SpareMoneyViewModel
        {
            Columns = _columnNames,
            Rows =
            [
                new List<object>
                {
                    $"£{balanceAfterDeduction}"
                }
            ]
        };
    }
}