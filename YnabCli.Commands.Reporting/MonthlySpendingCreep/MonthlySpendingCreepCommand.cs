namespace YnabCli.Commands.Reporting.MonthlySpendingCreep;

public class MonthlySpendingCreepCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string CategoryId = "category-id";
    }
    
    public Guid? CategoryId { get; set; }
}