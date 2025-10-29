using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCliCommand : ICliCommand
{
    public static class ArgumentNames
    {
        public const string CategoryId = "category-id";
    }
    
    public Guid? CategoryId { get; set; }
}