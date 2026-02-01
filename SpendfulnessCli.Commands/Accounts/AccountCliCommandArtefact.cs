using KitCli.Commands.Abstractions.Artefacts;
using YnabSharp;

namespace SpendfulnessCli.Commands.Accounts;

public class AccountCliCommandArtefact(Account account)
    : ValuedCliCommandArtefact<Account>(account.Name, account);