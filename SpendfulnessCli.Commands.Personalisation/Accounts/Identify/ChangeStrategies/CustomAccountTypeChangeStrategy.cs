using Spendfulness.Database.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

public class CustomAccountTypeChangeStrategy : IAccountAttributeChangeStrategy
{
    private readonly CustomAccountTypeRepository _customAccountTypeRepository;

    public CustomAccountTypeChangeStrategy(CustomAccountTypeRepository customAccountTypeRepository)
    {
        _customAccountTypeRepository = customAccountTypeRepository;
    }

    public bool AttributeHasChangedSince(AccountsIdentifyCliCommand command, CustomAccountAttributes accountAttributes)
    {
        var typeNamePresent = !string.IsNullOrEmpty(command.CustomAccountTypeName);
        var typeNameDifferent = command.CustomAccountTypeName != accountAttributes.CustomAccountType?.Name;
        
        return typeNamePresent && typeNameDifferent;
    }

    // TODO: Handle the cancelation token.
    public async Task<AccountAttributeChange> ChangeAttribute(AccountsIdentifyCliCommand command, CustomAccountAttributes attributes)
    {
        var customAccountType = await _customAccountTypeRepository.Find(command.CustomAccountTypeName!, CancellationToken.None);

        attributes.CustomAccountType = customAccountType;
        
        return new AccountAttributeChange(
            nameof(CustomAccountAttributes.CustomAccountType),
            attributes.CustomAccountType.Name,
            customAccountType.Name);
    }
}