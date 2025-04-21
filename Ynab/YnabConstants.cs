namespace Ynab;

public static class YnabConstants
{
    public static readonly List<string> AutomatedPayeeNames =
    [
        "Starting Balance",
        "Manual Balance Adjustment",
        "Reconciliation Balance Adjustment"
    ];
    
    public static Guid SplitCategoryId => Guid.Parse("26330e86-4711-41f9-bd3e-a1c983da936a");
}