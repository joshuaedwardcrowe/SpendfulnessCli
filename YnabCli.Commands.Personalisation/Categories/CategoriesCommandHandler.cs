using Cli.Commands.Abstractions;
using Cli.Outcomes;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Personalisation.Categories;

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