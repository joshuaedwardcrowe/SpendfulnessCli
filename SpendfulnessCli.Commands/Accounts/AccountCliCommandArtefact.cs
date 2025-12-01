using Cli.Commands.Abstractions.Artefacts;
using Ynab;

namespace SpendfulnessCli.Commands.Accounts;

public class AccountCliCommandArtefact(Account account)
    : ValuedCliCommandArtefact<Account>(account.Name, account);