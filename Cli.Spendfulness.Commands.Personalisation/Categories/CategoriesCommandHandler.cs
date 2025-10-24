using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Categories;

public class CategoriesCommandHandler: CommandHandler, ICommandHandler<CategoriesCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly CategoryCliTableBuilder _categoryCliTableBuilder;

    public CategoriesCommandHandler(ConfiguredBudgetClient configuredBudgetClient, CategoryCliTableBuilder categoryCliTableBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _categoryCliTableBuilder = categoryCliTableBuilder;
    }

    public async Task<CliCommandOutcome> Handle(CategoriesCommand request, CancellationToken cancellationToken)
    {
        var budget = await _configuredBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        
        var aggregator = new CategoryYnabAggregator(categoryGroups);

        var viewModel = _categoryCliTableBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}