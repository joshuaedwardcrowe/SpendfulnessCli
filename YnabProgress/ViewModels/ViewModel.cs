namespace YnabProgress.ViewModels;

public class ViewModel
{
    public List<string> Columns { get; set; } = [];
    public List<List<object>> Rows { get; set; } = [];
    public long? CompiledIn { get; set; }
}