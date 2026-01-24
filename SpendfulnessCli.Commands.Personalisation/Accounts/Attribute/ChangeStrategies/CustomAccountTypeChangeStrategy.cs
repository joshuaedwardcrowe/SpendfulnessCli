using Spendfulness.Database.Sqlite.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Attribute.ChangeStrategies;

public class CustomAccountTypeChangeStrategy : IAccountAttributeChangeStrategy
{
    private readonly CustomAccountTypeRepository _customAccountTypeRepository;

    public CustomAccountTypeChangeStrategy(CustomAccountTypeRepository customAccountTypeRepository)
    {
        _customAccountTypeRepository = customAccountTypeRepository;
    }

    public bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes)
    {
        var typeNamePresent = !string.IsNullOrEmpty(command.CustomAccountTypeName);
        var typeNameDifferent = command.CustomAccountTypeName != accountAttributes.CustomAccountType?.Name;
        
        return typeNamePresent && typeNameDifferent;
    }

    // TODO: Handle the cancelation token.
    public async Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes)
    {
        var customAccountType = await _customAccountTypeRepository.Find(command.CustomAccountTypeName!, CancellationToken.None);

        attributes.CustomAccountType = customAccountType;
        
        return new AccountAttributeChange(
            nameof(CustomAccountAttributes.CustomAccountType),
            attributes.CustomAccountType.Name,
            customAccountType.Name);
    }
}