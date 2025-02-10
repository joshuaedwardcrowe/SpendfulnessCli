namespace Ynab;

public abstract class YnabEvaluator<TEvaluationResult>(
    IEnumerable<Account>? accounts = null,
    IEnumerable<CategoryGroup>? categoryGroups = null,
    IEnumerable<Category>? categories = null,
    IEnumerable<ScheduledTransaction>? scheduledTransactions = null,
    IEnumerable<Transaction>? transactions = null)
    where TEvaluationResult : notnull
{
    protected List<Account> Accounts { get; set; } = accounts?.ToList() ?? [];
    protected List<CategoryGroup> CategoryGroups { get; set; } = categoryGroups?.ToList() ?? [];
    protected List<Category> Categories { get; set; } = categories?.ToList() ?? [];
    protected List<ScheduledTransaction> ScheduledTransactions { get; set; } = scheduledTransactions?.ToList() ?? [];
    protected List<Transaction> Transactions { get; set; } = transactions?.ToList() ?? [];

    public abstract TEvaluationResult Evaluate();
}