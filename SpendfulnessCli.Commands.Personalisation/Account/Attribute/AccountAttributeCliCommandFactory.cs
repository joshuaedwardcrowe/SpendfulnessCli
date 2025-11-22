using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Workflow.Commands.MissingOutcomes;

namespace SpendfulnessCli.Commands.Personalisation.Account.Attribute;

[FactoryFor(typeof(AccountCliCommand))]
public class AccountAttributeCliCommandFactory : ICliCommandFactory<AccountAttributeCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> properties)
        => instruction.SubInstructionName == AccountCliCommand.SubCommandNames.Attribute;
    
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var ynabAccountName = GetYnabAccountNameArgument(instruction, properties);
        if (ynabAccountName == null)
        {
            return new MissingOutcomesCliCommand([
                AccountCliCommand.ArgumentNames.AccountId,
                nameof(AccountCliCommandOutcome)
            ]);
        }
        
        var typeArgument = instruction.Arguments
            .OfType<string>(AccountAttributeCliCommand.ArgumentNames.Type);
        
        var interestRateArgument = instruction.Arguments
            .OfType<decimal>(AccountAttributeCliCommand.ArgumentNames.InterestRate);

        return new AccountAttributeCliCommand(
            ynabAccountName,
            typeArgument?.ArgumentValue,
            interestRateArgument?.ArgumentValue);
    }
    
    private string? GetYnabAccountNameArgument(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var ynabAccountNameArgument = instruction
            .Arguments
            .OfType<string>(AccountAttributeCliCommand.ArgumentNames.YnabAccountName);
        
        if (ynabAccountNameArgument != null)
        {
            return ynabAccountNameArgument.ArgumentValue;
        }
        
        var accountProperty = properties
            .OfType<AccountCliCommandArtefact>()
            .LastOrDefault();

        return accountProperty?.Value.Name;
    }
}