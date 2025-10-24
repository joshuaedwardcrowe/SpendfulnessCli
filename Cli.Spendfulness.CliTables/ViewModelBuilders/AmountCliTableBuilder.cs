using Cli.Spendfulness.CliTables.Formatters;

namespace Cli.Spendfulness.CliTables.ViewModelBuilders;

public class AmountCliTableBuilder : CliTableBuilder<decimal>
{
    private decimal? _minus;

    // TODO: I dont like this!
    public AmountCliTableBuilder WithSubtraction(decimal minus)
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