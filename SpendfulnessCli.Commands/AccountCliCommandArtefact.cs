using Cli.Commands.Abstractions.Artefacts;
using Ynab;

namespace SpendfulnessCli.Commands;

public class AccountCliCommandArtefact(Account account) : ValuedCliCommandArtefact<Account>(account);