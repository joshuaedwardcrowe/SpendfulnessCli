using YnabProgress.ViewModels;

namespace YnabProgress.Compilers;

public interface IViewModelCompiler<in TDataSet> where TDataSet : class
{
    public ViewModel Compile(TDataSet amountsPerYear);
}