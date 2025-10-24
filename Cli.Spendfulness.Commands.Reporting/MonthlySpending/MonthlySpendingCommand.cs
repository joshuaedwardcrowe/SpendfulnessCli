using Cli.Commands.Abstractions;

namespace Cli.Ynab.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string CategoryId = "category-id";
    }
    
    public Guid? CategoryId { get; set; }
}