using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.Users;
using SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify;

public class AccountsIdentifyCliCommandHandler : CliCommandHandler, ICliCommandHandler<AccountsIdentifyCliCommand>
{
    private readonly SpendfulnessBudgetClient _budgetClient;
    private readonly IEnumerable<IAccountAttributeChangeStrategy> _changeStrategies;
    private readonly CustomAccountAttributeRepository _customAccountAttributeRepository;
    private readonly UserRepository _userRepository;

    public AccountsIdentifyCliCommandHandler(
        SpendfulnessBudgetClient budgetClient,
        IEnumerable<IAccountAttributeChangeStrategy> changeStrategies,
        CustomAccountAttributeRepository customAccountAttributeRepository,
        UserRepository userRepository)
    {
        _budgetClient = budgetClient;
        _changeStrategies = changeStrategies;
        _customAccountAttributeRepository = customAccountAttributeRepository;
        _userRepository = userRepository;
    }

    public async Task<CliCommandOutcome[]> Handle(AccountsIdentifyCliCommand command, CancellationToken cancellationToken)
    {
        var attribute = await _customAccountAttributeRepository.Get(command.YnabAccountName, cancellationToken);
        
        if (attribute is null)
        {
            attribute = await CreateCustomAccountAttribute(command, cancellationToken);
        }

        var strategyTasks = _changeStrategies
            .Where(strategy => strategy.AttributeHasChangedSince(command, attribute))
            .Select(strategy => strategy.ChangeAttribute(command, attribute));
        
        var accountAttributeChanges = await Task.WhenAll(strategyTasks);
        
        // TODO: Create CliTable for AccountAttributeChange.
        return OutcomeAs($"Made {accountAttributeChanges.Length} Changes");
    }

    private async Task<CustomAccountAttributes> CreateCustomAccountAttribute(AccountsIdentifyCliCommand command, CancellationToken cancellationToken)
    {
        // Get the account
        var budget = await _budgetClient.GetDefaultBudget();
        var accounts = await budget.GetAccounts();

        var account = accounts.First(account => account.Name.IsSimilarTo(command.YnabAccountName));
        
        var activeUser = await _userRepository.FindActiveUser();

        var attribute =  new CustomAccountAttributes
        {
            YnabAccountId = account.Id,
            YnabAccountName = account.Name,
            // TODO: I assume this means it will insert it on save anyway.
            User = activeUser,
        };

        await _customAccountAttributeRepository.Save(attribute, cancellationToken);
        
        return attribute;
    }
}