using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Organisation.CopyOnBudget;

public class CopyOnBudgetCommand(Guid accountId) : ICommand
{
    public static class ArgumentNames
    {
        public const string AccountId = "account-id";
    }

    public Guid AccountId { get; init; } = accountId;
}