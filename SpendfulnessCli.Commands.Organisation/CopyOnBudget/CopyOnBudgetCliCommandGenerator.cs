using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Organisation.CopyOnBudget;

// TODO: Move me to personalisation class library.
public class CopyOnBudgetCliCommandGenerator : ICliCommandGenerator<CopyOnBudgetCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        var accountIdArgument = instruction.Arguments
            .OfRequiredType<Guid>(CopyOnBudgetCliCommand.ArgumentNames.AccountId);
        
        return new CopyOnBudgetCliCommand(accountIdArgument.ArgumentValue);
    }
}