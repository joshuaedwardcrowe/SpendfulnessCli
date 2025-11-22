using Cli.Commands.Abstractions.Properties;
using Ynab;

namespace SpendfulnessCli.Commands;

public class AccountCliCommandProperty(Account account) : ValuedCliCommandProperty<Account>(account);