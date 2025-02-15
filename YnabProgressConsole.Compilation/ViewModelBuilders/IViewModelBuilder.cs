using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public interface IViewModelBuilder<in TEvaluator, TEvaluation>
    where TEvaluator : YnabEvaluator<TEvaluation>
    where TEvaluation : notnull
{
    IViewModelBuilder<TEvaluator, TEvaluation> AddEvaluator(TEvaluator evaluator);
    
    IViewModelBuilder<TEvaluator, TEvaluation> AddColumnNames(List<string> columnNames);
    
    IViewModelBuilder<TEvaluator, TEvaluation> AddSortOrder(ViewModelSortOrder viewModelSortOrder);

    IViewModelBuilder<TEvaluator, TEvaluation> AddRowCount(bool showRowCount);

    ViewModel Build();
}