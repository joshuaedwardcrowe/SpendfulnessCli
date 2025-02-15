namespace Ynab;

public abstract class YnabEvaluator<TEvaluation>
{
    protected IEnumerable<Account> Accounts { get; set; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; set; }
    protected IEnumerable<Category> Categories { get; set; } 
    protected IEnumerable<ScheduledTransaction> ScheduledTransactions { get; set; }
    protected IEnumerable<Transaction> Transactions { get; set; }

    protected YnabEvaluator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = accounts;
        CategoryGroups = categoryGroups;
        Categories = new List<Category>();
        ScheduledTransactions = new List<ScheduledTransaction>();
        Transactions = new List<Transaction>();
    }

    protected YnabEvaluator(IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Categories = new List<Category>();
        ScheduledTransactions = new List<ScheduledTransaction>();
        Transactions = transactions;
    }

    public abstract TEvaluation Evaluate();
}