using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class CompanyDeductedBalanceViewModelBuilder : ViewModelBuilder<CategoryDeductedBalanceAggregator, decimal>
{
    protected override List<List<object>> BuildRows(decimal spareMoney)
    {
        var displayable = CurrencyDisplayFormatter.Format(spareMoney);
    
        return
        [
            new List<object>
            {
                displayable
            }
        ];
    }
}