using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public interface IViewModelBuilder<in TAggregator, TAggregation>
    where TAggregator : Aggregator<TAggregation>
    where TAggregation : notnull
{
    IViewModelBuilder<TAggregator, TAggregation> AddAggregator(TAggregator evaluator);
    
    IViewModelBuilder<TAggregator, TAggregation> AddColumnNames(List<string> columnNames);
    
    IViewModelBuilder<TAggregator, TAggregation> AddSortOrder(ViewModelSortOrder viewModelSortOrder);

    IViewModelBuilder<TAggregator, TAggregation> AddRowCount(bool showRowCount);

    ViewModel Build();
}