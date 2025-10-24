using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts;

public class AccountsCommandGenerator : ICommandGenerator<AccountsCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
        => subCommandName switch
        {
            AccountsCommand.SubCommandNames.Identify => GenerateIdentifyCommand(arguments),
            _ => new AccountsCommand()
        };

    private  AccountsIdentifyCommand GenerateIdentifyCommand(List<ConsoleInstructionArgument> arguments)
    {
        var nameArgument = arguments.OfRequiredType<string>(AccountsIdentifyCommand.ArgumentNames.Name);
        var typeArgument = arguments.OfRequiredType<string>(AccountsIdentifyCommand.ArgumentNames.Type);

        return new AccountsIdentifyCommand(nameArgument.ArgumentValue, typeArgument.ArgumentValue);
    }
}