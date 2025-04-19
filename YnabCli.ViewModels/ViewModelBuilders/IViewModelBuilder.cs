using YnabCli.Aggregation.Aggregator;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

public interface IViewModelBuilder<in TAggregator, TAggregation>
    where TAggregator : Aggregator<TAggregation>
    where TAggregation : notnull
{
    IViewModelBuilder<TAggregator, TAggregation> WithAggregator(TAggregator aggregator);
    
    IViewModelBuilder<TAggregator, TAggregation> WithSortOrder(ViewModelSortOrder viewModelSortOrder);

    IViewModelBuilder<TAggregator, TAggregation> WithRowCount(bool showRowCount);

    ViewModel Build();
}