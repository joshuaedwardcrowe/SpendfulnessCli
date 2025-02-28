using YnabCli.Commands.Generators;
using YnabCli.Commands.Personalisation.Accounts.Identify;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Accounts;

public class AccountsCommandGenerator : ICommandGenerator, ITypedCommandGenerator<AccountsCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        if (subCommandName == AccountsCommand.SubCommandNames.Identify)
        {
            return GenerateAccountIdentifyCommand(arguments);
        }
        
        return new AccountsCommand();
    }

    private AccountsIdentifyCommand GenerateAccountIdentifyCommand(List<InstructionArgument> arguments)
    {
        var nameArgument = arguments.OfRequiredType<string>(AccountsIdentifyCommand.ArgumentNames.Name);
        var typeArgument = arguments.OfRequiredType<string>(AccountsIdentifyCommand.ArgumentNames.Type);

        return new AccountsIdentifyCommand
        {
            YnabAccountName = nameArgument.ArgumentValue,
            AccountAccountTypeName = typeArgument.ArgumentValue,
        };
    }
}