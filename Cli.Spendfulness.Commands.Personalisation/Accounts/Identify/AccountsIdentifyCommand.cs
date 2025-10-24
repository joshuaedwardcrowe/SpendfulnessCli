using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

public record AccountsIdentifyCommand(string YnabAccountName, string CustomAccountTypeName) : ICommand
{
    public static class ArgumentNames
    {
        public const string Name = "name";
        public const string Type = "type";
    }
}