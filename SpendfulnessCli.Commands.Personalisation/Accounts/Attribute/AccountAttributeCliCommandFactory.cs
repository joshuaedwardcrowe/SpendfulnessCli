using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Workflow.Commands.MissingOutcomes;
using SpendfulnessCli.Commands.Accounts;
using Ynab;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Attribute;

public class AccountAttributeCliCommandFactory : ICliCommandFactory<AccountCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
        => instruction.SubInstructionName == AccountCliCommand.SubCommandNames.Attribute;
    
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var ynabAccountName = GetYnabAccountNameArgument(instruction, artefacts);
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

    private string? GetYnabAccountNameArgument(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var ynabAccountNameArgument = instruction
            .Arguments
            .OfType<string>(AccountAttributeCliCommand.ArgumentNames.YnabAccountName);

        if (ynabAccountNameArgument != null)
        {
            return ynabAccountNameArgument.ArgumentValue;
        }

        var accountArtefact = artefacts.OfType<Account>();

        return accountArtefact?.ArtefactValue.Name;
    }
}