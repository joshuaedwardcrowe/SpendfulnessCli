using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Compilers;

public interface IViewModelCompiler<in TDataSet> 
{
    public ViewModel Compile(TDataSet data);
}