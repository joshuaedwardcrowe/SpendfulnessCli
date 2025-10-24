using Cli.Commands.Abstractions;
using Cli.Outcomes;
using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregator;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.FlagChanges;

public class FlagChangesCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<FlagChangesCommand>
{
    public async Task<CliCommandOutcome> Handle(FlagChangesCommand command, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthFlaggedYnabAggregator(categoryGroups, transactions);
            
        if (command.From.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterFrom(command.From.Value));
        }

        if (command.To.HasValue)
        {
            aggregator.BeforeAggregation(t => t.FilterTo(command.To.Value));
        }
        
        var viewModel = new TransactionMonthFlaggedCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}