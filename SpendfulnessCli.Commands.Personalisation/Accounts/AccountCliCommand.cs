using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Accounts;

public record AccountCliCommand(Guid AccountId) : CliCommand
{
    public static class ArgumentNames
    {
        /// <summary>
        /// Guid of the account to identify.
        /// </summary>
        public const string AccountId = "id";
    }
    
    public static class SubCommandNames
    {
        /// <summary>
        /// Attribute detail to an account.
        /// </summary>
        public const string Attribute = "attribute";
    }

    public Guid AccountId { get; set; } = AccountId;
}