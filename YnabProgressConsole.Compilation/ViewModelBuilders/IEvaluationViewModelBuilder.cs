using Ynab;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public interface IEvaluationViewModelBuilder<in TEvaluator, TEvaluation> : IViewModelBuilder
    where TEvaluator : YnabEvaluator<TEvaluation>
    where TEvaluation : notnull
{
    IEvaluationViewModelBuilder<TEvaluator, TEvaluation> AddEvaluator(TEvaluator evaluator);
}