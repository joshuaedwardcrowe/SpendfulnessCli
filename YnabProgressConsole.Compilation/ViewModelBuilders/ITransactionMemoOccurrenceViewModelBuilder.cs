using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public interface ITransactionMemoOccurrenceViewModelBuilder :
    IEvaluationViewModelBuilder<TransactionMemoOccurrenceEvaluator, IEnumerable<TransactionMemoOccurrenceAggregate>>
{
    ITransactionMemoOccurrenceViewModelBuilder AddPayeeNameFilter(string payeeNameFilter);
    
    ITransactionMemoOccurrenceViewModelBuilder AddMinimumOccurrencesFilter(int minimumOccurrences);
}