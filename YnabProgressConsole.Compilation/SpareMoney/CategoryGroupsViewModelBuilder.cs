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
        var all = _spareMoneyAggregation.Aggregation.Sum(o => o.Balance);
        var result = all - _spareMoneyAggregation.AmountToIgnore;

        var rows = new List<List<object>>
        {
            new()
            {
                $"£{result}"
            }
        };
        
        return new SpareMoneyViewModel
        {
            Columns = _columnNames,
            Rows = rows,
        };
    }
}