using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Categories;

public class CategoriesCliCliCommandHandler: CliCommandHandler, ICliCommandHandler<CategoriesCliCommand>
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;
    private readonly CategoryCliTableBuilder _categoryCliTableBuilder;

    public CategoriesCliCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient, CategoryCliTableBuilder categoryCliTableBuilder)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
        _categoryCliTableBuilder = categoryCliTableBuilder;
    }

    public async Task<CliCommandOutcome> Handle(CategoriesCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        
        var aggregator = new CategoryYnabAggregator(categoryGroups);

        var viewModel = _categoryCliTableBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}