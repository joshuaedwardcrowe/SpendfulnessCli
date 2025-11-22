using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Personalisation.Account;

[FactoryFor(typeof(AccountCliCommand))]
public class AccountCliCommandFactory : ICliCommandFactory<AccountCliCommand>
{
    public bool CanGenerateWhen(CliInstruction instruction, List<CliCommandProperty> properties)
        => instruction.SubInstructionName is null;

    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var accountIdArgument = instruction.Arguments
            .OfRequiredType<Guid>(AccountCliCommand.ArgumentNames.AccountId);

        return new AccountCliCommand(accountIdArgument.ArgumentValue);
    }
}