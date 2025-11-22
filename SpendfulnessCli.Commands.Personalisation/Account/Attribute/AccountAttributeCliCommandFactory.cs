using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Workflow.Commands.MissingOutcomes;

namespace SpendfulnessCli.Commands.Personalisation.Account.Attribute;

[FactoryFor(typeof(AccountCliCommand))]
public class AccountAttributeCliCommandFactory : ICliCommandFactory<AccountAttributeCliCommand>
{
    public bool CanGenerateWhen(CliInstruction instruction, List<CliCommandProperty> properties)
        => instruction.SubInstructionName == AccountCliCommand.SubCommandNames.Attribute;
    
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
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
    
    
    private string? GetYnabAccountNameArgument(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var ynabAccountNameArgument = instruction
            .Arguments
            .OfType<string>(AccountAttributeCliCommand.ArgumentNames.YnabAccountName);
        
        if (ynabAccountNameArgument != null)
        {
            return ynabAccountNameArgument.ArgumentValue;
        }
        
        var accountProperty = properties
            .OfType<AccountCliCommandProperty>()
            .LastOrDefault();

        return accountProperty?.Value.Name;
    }
}