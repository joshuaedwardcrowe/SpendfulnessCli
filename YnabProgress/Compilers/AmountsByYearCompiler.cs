using Ynab.Collections;
using YnabProgress.Calculators;
using YnabProgress.ViewModels;

namespace YnabProgress.Compilers;

public class AmountsByYearCompiler : IViewModelCompiler<IEnumerable<AmountByYear>>
{
    public ViewModel Compile(IEnumerable<AmountByYear> amountsPerYear)
    {
        var viewModel = new ViewModel();
        
        CompileColumns(viewModel);

        var amountsPerYearIndexed = amountsPerYear.ToList();
        var firstAmountByYear = amountsPerYearIndexed.First();
        
        CompileFirstRow(viewModel, firstAmountByYear);

        for (var i = 1; i < amountsPerYearIndexed.Count; i++)
        {
            var prior = amountsPerYearIndexed.ElementAt(i - 1);
            var current = amountsPerYearIndexed.ElementAt(i);
            
            CompileOtherRow(viewModel, prior, current);
        }

        return viewModel;
    }
    
    private static void CompileColumns(ViewModel viewModel)
    {
        viewModel.Columns.AddRange(new List<string>
        {
            "Year",
            "Amount",
            "% Increase"
        });
    }

    private static void CompileFirstRow(ViewModel viewModel, AmountByYear firstAmountByYear)
    {
        viewModel.Rows.Add([
            firstAmountByYear.Year,
            firstAmountByYear.AverageAmount,
            "0%"
        ]);
    }

    private static void CompileOtherRow(
        ViewModel viewModel,
        AmountByYear priorAmountBYear,
        AmountByYear currentAmountByYear)
    {
        var row = new List<object>
        {
            currentAmountByYear.Year,
            $"£{currentAmountByYear.AverageAmount}"
        };
        
        var rowValue = GenerateRowValue(
            priorAmountBYear.AverageAmount,
            currentAmountByYear.AverageAmount);
        
        row.Add($"{rowValue}%");
            
        viewModel.Rows.Add(row);
    }

    private static int GenerateRowValue(decimal priorAverageAmount, decimal currentAverageAmount)
    {
        if (priorAverageAmount > currentAverageAmount)
        {
            return PercentageCalculator.CalculateChange(
                priorAverageAmount, 
                currentAverageAmount);
        }

        return PercentageCalculator.CalculateChange(
            currentAverageAmount, 
            priorAverageAmount);
    }
}