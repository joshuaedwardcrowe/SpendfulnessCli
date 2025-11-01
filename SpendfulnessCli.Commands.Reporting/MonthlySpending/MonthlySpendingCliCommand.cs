using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

public record MonthlySpendingCliCommand : CliCommand
{
    public static class ArgumentNames
    {
        public const string CategoryId = "category-id";
    }
    
    public Guid? CategoryId { get; set; }
}