using YnabCli.Aggregation.Aggregator.AmountAggregators;
using YnabCli.ViewModels.Formatters;

namespace YnabCli.ViewModels.ViewModelBuilders;

public class AmountViewModelBuilder : ViewModelBuilder<AmountAggregator, decimal>
{
    private decimal? _minus;

    public AmountViewModelBuilder WithSubtraction(decimal minus)
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