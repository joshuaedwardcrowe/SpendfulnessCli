namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class CopyOnBudgetCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string AccountId = "account-id";
    }

    public Guid AccountId { get; init; }
}