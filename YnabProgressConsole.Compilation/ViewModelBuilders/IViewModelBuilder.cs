using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public interface IViewModelBuilder
{
    public IViewModelBuilder AddColumnNames(List<string> columnNames);
    
    public IViewModelBuilder AddSortOrder(ViewModelSortOrder viewModelSortOrder);

    public IViewModelBuilder AddRowCount(bool showRowCount);

    public ViewModel Build();
}