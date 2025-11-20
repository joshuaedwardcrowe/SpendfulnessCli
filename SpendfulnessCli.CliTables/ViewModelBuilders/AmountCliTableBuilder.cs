using SpendfulnessCli.CliTables.Formatters;

namespace SpendfulnessCli.CliTables.ViewModelBuilders;

public class AmountCliTableBuilder : CliTableBuilder<decimal>
{
    private decimal? _minus;

    // TODO: I dont like this!
    public AmountCliTableBuilder WithSubtraction(decimal minus)
    {
        _minus = minus;
        return this;
    }
    
    protected override List<List<object>> BuildRows(IEnumerable<decimal> aggregates)
    {
        var sum = aggregates.Sum();
        var amount = sum - (_minus ?? 0);
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