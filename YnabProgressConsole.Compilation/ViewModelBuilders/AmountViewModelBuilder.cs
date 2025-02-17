using YnabProgressConsole.Compilation.Aggregator;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class AmountViewModelBuilder : ViewModelBuilder<CategoryDeductedBalanceAggregator, decimal>
{
    private decimal? _minus;

    public AmountViewModelBuilder AddMinus(decimal minus)
    {
        _minus = minus;
        return this;
    }
    
    protected override List<List<object>> BuildRows(decimal aggregates)
    {
        var amount = aggregates - (_minus ?? 0);
        var displayable = CurrencyDisplayFormatter.Format(amount);
    
        return
        [
            new List<object>
            {
                displayable
            }
        ];
    }
}