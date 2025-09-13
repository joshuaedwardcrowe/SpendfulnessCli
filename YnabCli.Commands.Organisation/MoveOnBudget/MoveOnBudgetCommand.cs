namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class MoveOnBudgetCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string AccountId = "account-id";
    }

    public Guid AccountId { get; init; }
}