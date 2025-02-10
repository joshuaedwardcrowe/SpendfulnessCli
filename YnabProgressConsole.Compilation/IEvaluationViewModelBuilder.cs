using Ynab;

namespace YnabProgressConsole.Compilation;

public interface IEvaluationViewModelBuilder<in TEvaluator, TEvaluation> : IViewModelBuilder
    where TEvaluator : YnabEvaluator<decimal>
{
    IEvaluationViewModelBuilder<TEvaluator, TEvaluation> AddEvaluator(TEvaluator evaluator);
}