using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts;

public class AccountsCommandGenerator : ICommandGenerator<AccountsCliCommand>
{
    public ICliCommand Generate(CliInstruction instruction)
        => instruction.SubInstructionName switch
        {
            AccountsCliCommand.SubCommandNames.Identify => GenerateIdentifyCommand(instruction.Arguments),
            
            // TODO: Defaulting to this creates a confusing message when you input an invalid sub command.
            // (maybe add a validator to the ICommandGenerator interface)
            _ => new AccountsCliCommand()
        };
    
    private  AccountsIdentifyCliCommand GenerateIdentifyCommand(List<CliInstructionArgument> arguments)
    {
        var nameArgument = arguments.OfRequiredType<string>(AccountsIdentifyCliCommand.ArgumentNames.Name);
        var typeArgument = arguments.OfType<string>(AccountsIdentifyCliCommand.ArgumentNames.Type);
        var interestRateArgument = arguments.OfType<decimal>(AccountsIdentifyCliCommand.ArgumentNames.InterestRate);

        return new AccountsIdentifyCliCommand(
            nameArgument.ArgumentValue,
            typeArgument?.ArgumentValue,
            interestRateArgument?.ArgumentValue);
    }
}