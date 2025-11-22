using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Organisation.CopyOnBudget;

// TODO: Move me to personalisation class library.
public class CopyOnBudgetCliCommandFactory : ICliCommandFactory<CopyOnBudgetCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var accountIdArgument = instruction.Arguments
            .OfRequiredType<Guid>(CopyOnBudgetCliCommand.ArgumentNames.AccountId);
        
        return new CopyOnBudgetCliCommand(accountIdArgument.ArgumentValue);
    }
}