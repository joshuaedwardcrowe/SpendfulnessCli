using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Categories;

public class CategoriesCliCommandHandler: CliCommandHandler, ICliCommandHandler<CategoriesCliCommand>
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;
    private readonly CategoryCliTableBuilder _categoryCliTableBuilder;

    public CategoriesCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient, CategoryCliTableBuilder categoryCliTableBuilder)
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