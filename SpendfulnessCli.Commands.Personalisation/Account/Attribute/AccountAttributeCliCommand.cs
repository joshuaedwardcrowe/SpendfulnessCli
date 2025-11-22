using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Attribute;


// TODO: Rename me to 'Attribute'
public record AccountAttributeCliCommand(string YnabAccountName, string? CustomAccountTypeName, decimal? InterestRate) : CliCommand
{
    public static class ArgumentNames
    {
        /// <summary>
        /// Name of the account to identify.
        /// </summary>
        public const string YnabAccountName = "name";
        
        /// <summary>
        /// How the account should be identified.
        /// </summary>
        public const string Type = "type";
        
        /// <summary>
        /// What the interest rate is.
        /// </summary>
        public const string InterestRate = "interest-rate";
    }
}