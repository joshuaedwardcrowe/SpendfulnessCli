namespace YnabCli.ViewModels.Calculators;

public static class PercentageCalculator
{
    public static int CalculateChange(decimal originalValue, decimal newValue)
    {
        // Avoid dividing by zero.
        if (originalValue == 0)
        {
            return 0;
        }
        
        // 60 - 40 = 20
        var difference = newValue - originalValue;
        

        // (20 / 60) * 100 = 33.33
        var percentage = difference / originalValue * 100;
        
        // (int)33.33 = 33
        return (int)percentage;
    }
}