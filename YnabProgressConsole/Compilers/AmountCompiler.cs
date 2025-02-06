using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Compilers;

public class AmountCompiler : IViewModelCompiler<decimal>
{
    public ViewModel Compile(decimal data)
    {
        var viewModel = new ViewModel();
        
        viewModel.Columns.Add("Amount");
        viewModel.Rows.Add([data]);

        return viewModel;
    }
}