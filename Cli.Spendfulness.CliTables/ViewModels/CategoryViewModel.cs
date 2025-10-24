namespace Cli.Spendfulness.CliTables.ViewModels;

public class CategoryViewModel
{
    public const string CategoryName = "Category Name";
    public const string CategoryId = "Category Id";
    
    public static List<string> GetColumnNames() 
        => [CategoryName, CategoryId];
}