using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;

namespace SpendfulnessCli.Commands.Personalisation.Categories;

public class CategoriesCliCommandHandler: CliCommandHandler, ICliCommandHandler<CategoriesCliCommand>
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;
    private readonly CategoryCliTableBuilder _categoryCliTableBuilder;

    public CategoriesCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient, CategoryCliTableBuilder categoryCliTableBuilder)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
        _categoryCliTableBuilder = categoryCliTableBuilder;
    }

    public async Task<CliCommandOutcome[]> Handle(CategoriesCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();
        
        var categoryGroups = await budget.GetCategoryGroups();
        
        var aggregator = new CategoryYnabAggregator(categoryGroups);

        var viewModel = _categoryCliTableBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return OutcomeAs(viewModel);
    }
}