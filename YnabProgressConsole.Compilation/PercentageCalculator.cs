namespace YnabProgressConsole.Compilation.Calculators;

public static class PercentageCalculator
{
    public static int CalculateChange(decimal biggerValue, decimal smallerValue)
    {
        
        // 60 - 40 = 20
        decimal difference = biggerValue - smallerValue;
        
        // Avoid dividing by zero.
        if (biggerValue == 0)
        {
            return 0;
        }

        // (20 / 60) * 100 = 33.33
        var percentage = (difference / biggerValue) * 100;
        
        // (int)33.33 = 33
        return (int)percentage;
    }
}