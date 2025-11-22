using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Accounts;

public class AccountCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is AccountCliCommandOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var accountOutcome = (AccountCliCommandOutcome)outcome;
        return new AccountCliCommandArtefact(accountOutcome.Account);
    }
}