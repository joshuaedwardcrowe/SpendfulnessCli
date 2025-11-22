using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Personalisation.Account;

[FactoryFor(typeof(AccountCliCommand))]
public class AccountCliCommandFactory : ICliCommandFactory<AccountCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> properties)
        => instruction.SubInstructionName is null;

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var accountIdArgument = instruction.Arguments
            .OfRequiredType<Guid>(AccountCliCommand.ArgumentNames.AccountId);

        return new AccountCliCommand(accountIdArgument.ArgumentValue);
    }
}