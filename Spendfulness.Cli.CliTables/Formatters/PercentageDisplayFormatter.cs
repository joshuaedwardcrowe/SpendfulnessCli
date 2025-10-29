namespace Spendfulness.Cli.CliTables.Formatters;

public static class PercentageDisplayFormatter
{
    public static string Format(decimal percentage) => $"{percentage}%";
}