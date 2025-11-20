using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Organisation.CopyOnBudget;

public record CopyOnBudgetCliCommand(Guid AccountId) : CliCommand
{
    public static class ArgumentNames
    {
        public const string AccountId = "account-id";
    }
}