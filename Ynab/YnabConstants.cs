namespace Ynab;

public static class YnabConstants
{
    public static readonly List<string> AutomatedPayeeNames =
    [
        "Starting Balance",
        "Manual Balance Adjustment",
        "Reconciliation Balance Adjustment"
    ];

    public static readonly List<string> AutomatedCategoryNames = new List<string>
    {
        "Inflow: Ready to Assign",
        "Split"
    };
}