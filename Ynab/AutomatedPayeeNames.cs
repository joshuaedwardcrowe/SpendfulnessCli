namespace Ynab;

public static class AutomatedPayeeNames
{
    public const string ManualBalanceAdjustment = "Manual Balance Adjustment";
    public const string ReconciliationBalanceAdjustment = "Reconciliation Balance Adjustment";
    
    public static readonly List<string> All =
    [
        ManualBalanceAdjustment,
        ReconciliationBalanceAdjustment
    ];
}