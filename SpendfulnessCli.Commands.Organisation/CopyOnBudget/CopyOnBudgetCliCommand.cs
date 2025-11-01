using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Organisation.CopyOnBudget;

public record CopyOnBudgetCliCommand(Guid accountId) : CliCommand
{
    public static class ArgumentNames
    {
        public const string AccountId = "account-id";
    }

    public Guid AccountId { get; init; } = accountId;
}